using AutoMapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using thuongmaidientus1.Common;
using thuongmaidientus1.EmailConfig;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly DBContexts _dbcontext;
        private readonly IMapper _mapper;
        private readonly EmailSetting _emaiSetting;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly Jwt _jwt;
        private readonly IUserService _userService;
        public AccountService(DBContexts dbcontext, IMapper mapper, IOptions<EmailSetting> emailSetting, IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IOptionsMonitor<Jwt> jwt, IUserService userService)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _emaiSetting = emailSetting.Value;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _jwt = jwt.CurrentValue;
            _userService = userService;
        }

        public async Task<string> Action(string code)
        {
            var message = "";
            var checkCode = _dbcontext.tokens.Include(x => x.account).Where(x => x.token == code).FirstOrDefault();
            if (checkCode == null)
            {
                message = "Error";
                return await Task.FromResult(message);
            }

            var checkAccount = _dbcontext.UserRegMsts.Where(x => x.Equals(checkCode.account)).FirstOrDefault();
            if (checkAccount == null)
            {
                message = "Tài khoản không tồn tại";
                return await Task.FromResult(message);
            }

            var removeToken = _dbcontext.tokens.Where(x => x.account.Equals(checkCode.account)).ToList();

            checkAccount.Action = true;
            _dbcontext.UserRegMsts.Update(checkAccount);
            _dbcontext.tokens.RemoveRange(removeToken);
            if (await _dbcontext.SaveChangesAsync() > 0)
            {
                message = "Acction Thành công";
            }
            return await Task.FromResult(message);
        }

        public async Task<AccountDTO> AddAccount(AccountDTO accountDTO)
        {
            // Kiểm tra xem tài khoản đã tồn tại chưa
            var existingAccount = _dbcontext.UserRegMsts.FirstOrDefault(x => x.username == accountDTO.username && x.email == accountDTO.email);

            /* "Include(a => a.account)" Câu lệnh này là liên kết giữa 2 bảng "token" và bảng "account", ở đây bảng "token" đang liên kết đến bảng "account",
                biến đại diện cho bảng "account" ở đây là "a"
                
                "ThenInclude(r => r.role)" Câu lệnh này nghĩa là liên kết giữa bảng "account"(trước đó đã sử dụng hàm "Include(a => a.account)" để liên kết) 
                với bảng "role", ở đây bảng "account" đang liên kết đến bảng "role", và biến đại diện cho bảng "role" ở đây là "r"

                Tóm tắt giải thích câu lệnh này: 

                "var checkTokenAccount = _dbcontext.tokens.Include(a => a.account).
                ThenInclude(r => r.role).
                Where(x => x.token == accountDTO.token).
                FirstOrDefault();"
                
                Câu lệnh này đang liên kết 3 bảng với nhau, và muốn liên kết được 3 bảng thì sử dụng phương thức "ThenInclude()" để liên kết nhiều hơn 2 bảng
                
             */
            var checkTokenAccount = _dbcontext.tokens.Include(a => a.account).
                ThenInclude(r => r.role).
                Where(x => x.token == accountDTO.token).
                FirstOrDefault();

            var role = _dbcontext.roles.Where(x => x.name == "User").FirstOrDefault();
            var newAccount = _mapper.Map<UserRegMst>(accountDTO);
            if (existingAccount != null)
            {
                return null; // Tài khoản đã tồn tại, trả về null hoặc thông báo lỗi khác
            }
            if (checkTokenAccount != null)
            {
                var checkRoleAccount = _dbcontext.roles.Where(x => x.Equals(checkTokenAccount.account.role)).FirstOrDefault();
                // Tạo tài khoản và lưu vào cơ sở dữ liệu
                
                if (checkRoleAccount.name == "Admin")
                {
                    var checkRole = _dbcontext.roles.Where(x => x.id == accountDTO.role_id).FirstOrDefault();
                    if (checkRole != null)
                    {
                        newAccount.role = checkRole;
                    }
                    else
                    {
                        newAccount.role = role;
                    }
                    newAccount.Action = accountDTO.Action;
                    newAccount.Deleted = accountDTO.Action == true ? false : true;

                }

                if (checkRoleAccount.name != "Admin")
                {
                    var roleUser = _dbcontext.roles.Where(x => x.name == "User").FirstOrDefault();
                    newAccount.Action = false;
                    newAccount.Deleted = false;
                    newAccount.role = roleUser;
                }


                _dbcontext.UserRegMsts.Add(newAccount);

                if (_dbcontext.SaveChanges() <= 0)
                {
                    return null; // Lỗi khi lưu tài khoản vào cơ sở dữ liệu
                }

                if (checkRoleAccount.name != "Admin")
                {
                    var code = new Tokens
                    {
                        account = newAccount,
                        token = geneAction()
                    };

                    _dbcontext.tokens.Add(code);
                    await _dbcontext.SaveChangesAsync();

                    // Tạo nội dung email
                    var descriptEmail = new SendEmail
                    {
                        title = "Mã xác nhận tài khoản",
                        message = "Thông tin mã xác nhận",
                        active = code.token,
                        name = accountDTO.username,
                        iamge = newAccount.image
                    };

                    var view = "EmailTemplate";
                    var emailHTML = await RenderViewToStringAsync(view, descriptEmail);
                    await SendEmai(accountDTO.email, descriptEmail.title, emailHTML);

                    //await RenderViewToStringAsync(view, descriptEmail, accountDTO.email, descriptEmail.title);

                    //var body = "";
                    //await SendEmai(accountDTO.email, descriptEmail.title, body);
                }

            }
            else
            {
                newAccount.Action = false;
                newAccount.Deleted = false;
                newAccount.role = role;

                _dbcontext.UserRegMsts.Add(newAccount);

                if (_dbcontext.SaveChanges() <= 0)
                {
                    return null; // Lỗi khi lưu tài khoản vào cơ sở dữ liệu
                }

                var code = new Tokens
                {
                    account = newAccount,
                    token = geneAction()
                };

                _dbcontext.tokens.Add(code);
                await _dbcontext.SaveChangesAsync();

                // Tạo nội dung email
                var descriptEmail = new SendEmail
                {
                    title = "Mã xác nhận tài khoản",
                    message = "Thông tin mã xác nhận",
                    active = code.token,
                    name = accountDTO.username,
                    iamge = newAccount.image
                };

                var view = "EmailTemplate";
                var emailHTML = await RenderViewToStringAsync(view, descriptEmail);
                await SendEmai(accountDTO.email, descriptEmail.title, emailHTML);

                //await RenderViewToStringAsync(view, descriptEmail, accountDTO.email, descriptEmail.title);

                //var body = "";
                //await SendEmai(accountDTO.email, descriptEmail.title, body);
            }

            

            // Trả về thông tin tài khoản mới được tạo
            return await Task.FromResult(new AccountDTO
            {
                username = newAccount.username,
                email = newAccount.email
            });
        }

        public async Task SendEmai(string emai, string tieude, string body)
        {
            // ĐOẠN LỆNH NÀY LÀ GÁN NỘI DUNG CỦA TRANG "cshtml" vào nội dụng Email để gửi
            /*
                "var bodyBuilder = new BodyBuilder();": Đầu tiên, bạn tạo một đối tượng BodyBuilder. 
                Đây là một lớp trong thư viện MimeKit được sử dụng để xây dựng nội dung của email.
                
                "bodyBuilder.Html = body;": Sau đó, bạn gán nội dung HTML cho thuộc tính Html của đối tượng bodyBuilder. 
                Trong ví dụ này, nội dung HTML ở đây là "body", "body" ở đây là một trang "cshtml". Điều này đại diện cho nội dung chính của email, 
                và nó sẽ được hiển thị bằng định dạng HTML khi người nhận mở email.
                
                "message.Body = bodyBuilder.ToMessageBody();": Cuối cùng, bạn gán nội dung được tạo bằng bodyBuilder vào thuộc tính Body của đối tượng
                message. Điều này đặt nội dung của email trở thành nội dung bạn đã xác định bằng HTML.

                Kết quả cuối cùng là message chứa nội dung email với định dạng HTML mà bạn đã xác định trong bodyBuilder. 
                Nó sẽ được gửi đến người nhận dưới dạng email với nội dung HTML.
             */
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emaiSetting.SenderName, _emaiSetting.SenderEmail));
            message.To.Add(new MailboxAddress("", emai));
            message.Subject = tieude;
            message.Body = bodyBuilder.ToMessageBody();
            //message.Body = new TextPart("html")
            //{aa
            //    Text = body
            //};

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emaiSetting.SmtpServer, _emaiSetting.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emaiSetting.Username, _emaiSetting.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        private string geneAction()
        {
            var random = new Random();
            string code = "QWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            var geneCode = new string(Enumerable.Repeat(code, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            return geneCode;
        }

        public async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            string message = "";
            using (var scope = _serviceProvider.CreateScope())
            {
                var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
                viewData.Model = model;

                var actionContext = new ActionContext(
                    new DefaultHttpContext { RequestServices = scope.ServiceProvider },
                    new RouteData(),
                    new ActionDescriptor()
                );

                using (var writers = new StringWriter())
                {
                    try
                    {
                        var viewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: false);
                        var viewContext = new ViewContext(actionContext, viewResult.View, viewData, new TempDataDictionary(actionContext.HttpContext, _tempDataProvider), writers, new HtmlHelperOptions());

                        await viewResult.View.RenderAsync(viewContext);
                        return await Task.FromResult(writers.ToString());
                        //await SendEmai(email, title);
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi, ví dụ: log lỗi hoặc thông báo cho người dùng
                        message = $"{ex.Message}";
                    }
                }
            }
            return await Task.FromResult(message);
        }
        public async Task<string> ConvertToHS256(string value)
        {
            string key = "ThisismySecretKey";

            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] stringToByte = Encoding.UTF8.GetBytes(value);

            string MaHoa = "";
            using (HMACSHA256 Hm = new HMACSHA256(keyByte))
            {
                byte[] GiaiMaHoa = Hm.ComputeHash(stringToByte);
                MaHoa = Convert.ToBase64String(GiaiMaHoa);
            }

            return await Task.FromResult(MaHoa);
        }

        public async Task<string> DecryptHS256(string encodedValue)
        {
            string check = await DecryptHS256s(encodedValue);


            return await Task.FromResult(check);
        }

        public async Task<string> DecryptHS256s(string encodedValue)
        {
            string key = "ThisismySecretKey";

            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] encodedValueByte = Encoding.UTF8.GetBytes(encodedValue); // Chuyển đổi chuỗi Base64 thành mảng byte

            string decryptedValue = "";
            using (HMACSHA256 Hm = new HMACSHA256(keyByte))
            {
                byte[] computedHash = Hm.ComputeHash(encodedValueByte);

                // So sánh mã băm tính được với mã băm ban đầu để xác minh tính toàn vẹn
                bool hashesMatch = encodedValueByte.SequenceEqual(computedHash);

                if (hashesMatch)
                {
                    // Mã băm đúng, giải mã dữ liệu
                    decryptedValue = Convert.ToBase64String(encodedValueByte);
                }
                else
                {
                    // Mã băm không khớp, xử lý lỗi hoặc thông báo về tính toàn vẹn dữ liệu
                    decryptedValue = "Mã băm không khớp. Dữ liệu có thể đã bị sửa đổi.";
                }
            }

            return await Task.FromResult(decryptedValue);
        }

        public async Task<string> DeleteAccount(IList<string> id)
        {
            string message = "";
            if (!id.Any() || id == null)
                return null;


            try
            {
                foreach (var item in id)
                {
                    bool checkInt = Regex.IsMatch(item, @"^\d+$");
                    if (int.TryParse(item, out int songuyen))
                    {
                        var checkIdList = _dbcontext.UserRegMsts.Where(x => x.id == songuyen).ToList();
                        if(checkIdList != null)
                        {
                            for (var i = 0; i < checkIdList.Count(); i++)
                            {
                                var data = checkIdList[i];
                                data.Deleted = true;
                                _dbcontext.UserRegMsts.Update(data);
                            }
                        }
                        //_dbcontext.UserRegMsts.RemoveRange(checkIdList);
                    }
                    else if(!checkInt) {
                        var chuoi = item.Trim();
                        //var checkList = _dbcontext.UserRegMsts.Where(x => x.username.Contains(chuoi)).ToList();
                        var checkList = _dbcontext.UserRegMsts.Where(x => x.username == chuoi).ToList();
                        if(checkList != null)
                        {
                            for(var i = 0; i < checkList.LongCount(); i++)
                            {
                                var data = checkList[i];
                                data.Deleted = true;
                                _dbcontext.UserRegMsts.Update(data);
                            }
                        }
                        //_dbcontext.UserRegMsts.RemoveRange(checkList);
                    }
                    //else if (string.IsNullOrWhiteSpace(item) && !string.IsNullOrEmpty(item))
                    //{
                    //var chuoi = item.Trim();
                    ////var checkList = _dbcontext.UserRegMsts.Where(x => x.username.Contains(chuoi)).ToList();
                    //var checkList = _dbcontext.UserRegMsts.Where(x => x.username == chuoi).ToList();
                    //_dbcontext.UserRegMsts.RemoveRange(checkList);
                    //}

                }

                if (await _dbcontext.SaveChangesAsync() > 0)
                {
                    message = "OK";
                }

            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return await Task.FromResult(message);
        }

        public async Task<AccountDTO> EditAccount(int id, AccountDTO accountDTO)
        {
            try
            {
                var checkToken = _dbcontext.tokens.Include(a => a.account).Where(x => x.token == accountDTO.token).FirstOrDefault();
                if(checkToken == null)
                {
                    return await Task.FromResult(new AccountDTO { username = "Chưa có token" });
                }
                var checkAccountToken = _dbcontext.UserRegMsts.Where(x => x.Equals(checkToken.account)).FirstOrDefault();
                char[] kytu = new char[] { ',' };

                // Cắt chuỗi theo dấu ",", "Select(part => part.Trim())" là xóa khoảng trắng
                string[] chuoicat = accountDTO.password.Split(kytu, StringSplitOptions.RemoveEmptyEntries).Select(part => part.Trim()).ToArray();
                if(chuoicat.Length <= 1)
                {
                    return await Task.FromResult(new AccountDTO { username = "Chưa nhập mật khẩu mới" });
                }

                
                var checkId = _dbcontext.UserRegMsts.Where(x => x.id == accountDTO.id).FirstOrDefault();
                var checkAccountName = _dbcontext.UserRegMsts.Where(x => x.username != checkId.username && x.username == accountDTO.username).FirstOrDefault();
                
                if(checkAccountName != null) {
                    return await Task.FromResult(new AccountDTO { username = "Null" });
                }
                //var checkName = _dbcontext.UserRegMsts.Where(x => x.username == accountDTO.username).FirstOrDefault();

                //if (checkName != null && accountDTO.username != checkId.username)
                //{
                //    return await Task.FromResult(new AccountDTO { username = "Null" });
                //}
                if (checkId == null)
                {
                    return await Task.FromResult(new AccountDTO { username = "Null" });
                }

                // Lấy ra mật khẩu mới được chuyền lên
                for (var i = 0; i < chuoicat.Length; i++)
                {
                    var data = chuoicat[i];
                    if (i == 1)
                    {
                        accountDTO.password = data;
                    }
                }

                var checkAccounts = _dbcontext.UserRegMsts.Where(x => x.username == accountDTO.username && x.password == accountDTO.password).FirstOrDefault();
                if (checkAccounts == null)
                {
                    return await Task.FromResult(new AccountDTO { username = "Tài khoản không chính xác" });
                }



                if (accountDTO.role_id == null)
                {
                    string IdAccount = _userService.name();

                    var checkAccount = _dbcontext.UserRegMsts.Include(x => x.role).Where(x => x.id == int.Parse(IdAccount)).FirstOrDefault();
                    var checkRoles = _dbcontext.roles.Where(x => x.Equals(checkAccount.role)).FirstOrDefault();
                    checkId.username = accountDTO.username;
                    checkId.password = accountDTO.password;
                    checkId.email = accountDTO.email;
                    checkId.role = checkRoles;
                    checkId.image = accountDTO.image;
                    checkId.Action = accountDTO.Action;
                    checkId.CretorEdit = checkAccountToken.username + " là người chỉnh sửa bản ghi vào thời gian " + DateTimeOffset.UtcNow;
                    checkId.UpdatedAt = DateTimeOffset.UtcNow;
                    
                }
                else if(accountDTO.role_id != 0)
                {
                    var checkRoles = _dbcontext.roles.Where(x => x.id == accountDTO.role_id).FirstOrDefault();
                    if(checkRoles == null)
                    {
                        return await Task.FromResult(new AccountDTO { username = "Null" });
                    }

                    checkId.username = accountDTO.username;
                    checkId.password = accountDTO.password;
                    checkId.email = accountDTO.email;
                    checkId.phonenumber = accountDTO.phonenumber;
                    checkId.role = checkRoles;
                    checkId.image = accountDTO.image;
                    checkId.Action = accountDTO.Action;
                    checkId.Action = accountDTO.Action;
                    checkId.CretorEdit = checkAccountToken.username + " là người chỉnh sửa bản ghi vào thời gian " + DateTimeOffset.UtcNow;
                    checkId.UpdatedAt = DateTimeOffset.UtcNow;
                }
                _dbcontext.UserRegMsts.Update(checkId);
                if (await _dbcontext.SaveChangesAsync() > 0)
                {
                    return await Task.FromResult(new AccountDTO { id = checkId.id, username = accountDTO.username });
                }

            }
            catch (Exception e)
            {
                return await Task.FromResult(new AccountDTO { username = $"Error: {e.Message}" });
            }
            return await Task.FromResult(new AccountDTO { username = "Null" });
        }

        public async Task<PayLoad<object>> FindAll(string? name, int page = 1, int pageSize = 20)
        {
            var AccountRole = _dbcontext.UserRegMsts.Include(r => r.role).Where(x => !x.Deleted).Select(x => new
            {
                Id = x.id,
                Name = x.username,
                Password = x.password,
                Email = x.email,
                Phone = x.phonenumber,
                role = x.role.name,
                Action = x.Action == true ? "Action" : "UnAction",
                Image = x.image,
                CretorEdit = x.CretorEdit
            }).ToList();

            if (!string.IsNullOrEmpty(name))
                AccountRole = AccountRole.Where(x => x.Name.Contains(name)).ToList();

            var PageList = new PageList<object>(AccountRole, page - 1, pageSize);

            return await Task.FromResult(PayLoad<object>.Successfully(new
            {
                PageIndex = page,
                PageSize = PageList.pageSize,
                TotalCount = PageList.totalCounts,
                TotalPage = PageList.totalPages,
                Data = PageList
            }));

        }

        public async Task<AccountDTO> Login(Login login)
        {
            //string key = "ThisismySecretKey";
            var checkLogins = _dbcontext.UserRegMsts.Include(u => u.role).Where(x => (x.username == login.username || x.email == login.username) && x.Action && !x.Deleted).FirstOrDefault();
            //var checkLogins = from a in _dbcontext.UserRegMsts
            //                  join r in _dbcontext.roles on a.role.id equals r.id
            //                  where a.username == login.username && a.email == login.username
            //                  select new
            //                  {
            //                      username = a.username,
            //                      password = a.password,
            //                      roleId = r.id
            //                  };
            var checkLogin = _dbcontext.UserRegMsts.Where(x => !x.Deleted).ToList();

            byte[] codeDTO = Encoding.UTF8.GetBytes(_jwt.Key);
            byte[] StringToByteDto = Encoding.UTF8.GetBytes(login.password);

            string? ByteToStringDto = null;
            using (HMACSHA256 HmDTO = new HMACSHA256(codeDTO))
            {
                byte[] GiaiMaDTO = HmDTO.ComputeHash(StringToByteDto);
                ByteToStringDto = Convert.ToBase64String(GiaiMaDTO);
            }

            string? checkPassword = null;
            foreach (var item in checkLogin)
            {
                byte[] code = Encoding.UTF8.GetBytes(_jwt.Key);
                byte[] stringToByte = Encoding.UTF8.GetBytes(item.password);
                using (HMACSHA256 Hm = new HMACSHA256(code))
                {
                    byte[] GiaiMaHoa = Hm.ComputeHash(stringToByte);
                    checkPassword = Convert.ToBase64String(GiaiMaHoa);
                }

                if (checkPassword == ByteToStringDto && checkLogins.username == item.username)
                {
                    //var keys = new Jwt
                    //{
                    //    Key = "ThisismySecretKey",
                    //    Issuer = "MyDomain.com"
                    //};
                    var role = _dbcontext.roles.Where(x => x.Equals(checkLogins.role)).FirstOrDefault();
                    if (role == null)
                    {
                        return await Task.FromResult(new AccountDTO { username = "Null"});
                    }
                    var JwtHandler = new JwtSecurityTokenHandler();
                    var Sereckey = Encoding.UTF8.GetBytes(_jwt.Key);

                    var claim = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Email, item.email),
                        new Claim(ClaimTypes.Name, item.username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Lấy ra "id" của mã Token
                        new Claim("Id", item.id.ToString()),
                        new Claim(ClaimTypes.Role, role.name)
                    };

                    //var tokenDescript = new SecurityTokenDescriptor
                    //{
                    //    Subject = new ClaimsIdentity(claim),

                    //    // Thời gian hết hạn của token
                    //    Expires = DateTime.UtcNow.AddMinutes(2000000),

                    //    // Sử dụng mã hóa "HS256" để ký lên token
                    //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Sereckey), SecurityAlgorithms.HmacSha256),
                    //};

                    //// Lấy token 
                    //var token = JwtHandler.CreateToken(tokenDescript);
                    //var accessToken = JwtHandler.WriteToken(token); // Trả về chuỗi Token

                    var AddToken = new Tokens
                    {
                        account = item,
                        token = GenerateToken(claim),
                        geneToken = GenerateRefreshToken()
                    };

                    _dbcontext.tokens.Add(AddToken);
                    if (await _dbcontext.SaveChangesAsync() < 0)
                    {
                        return await Task.FromResult(new AccountDTO { username = "Null" });
                    }

                    var checkShop = _dbcontext.Inquirys.Include(s => s.account).Where(x => x.account.Equals(item) && !x.Deleted).FirstOrDefault();

                    return await Task.FromResult(new AccountDTO
                    {
                        id = item.id,
                        username = item.username,
                        email = item.email,
                        role_id = role.id,
                        role_Name = role.name,
                        image = item.image,
                        token = AddToken.token,
                        shop_id = checkShop == null ? 0 : checkShop.id,
                        shop_name = checkShop == null ? "" : checkShop.Name

                    });
                }


            }

            return await Task.FromResult(new AccountDTO { username = "Null" });

        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];

            using (var rng = RandomNumberGenerator.Create()) // Tạo chuỗi ngẫu nhiên
            {
                rng.GetBytes(random); // Gán chuỗi ngẫu nhiên vừa tạo cho mảng byte ở trên
                return Convert.ToBase64String(random); // Chuyển từ mảng byte sang string để trả về 
            }
        }

        private string GenerateToken(List<Claim>? claim)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creadentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_jwt.Issuer,
                _jwt.Issuer,
                expires: DateTime.Now.AddMinutes(12000),
                claims: claim,
                signingCredentials: creadentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AccountDTO> Logout(string name)
        {
            try
            {
                var checkAccount = _dbcontext.UserRegMsts.Where(x => x.username == name).FirstOrDefault();
                var checkLogout = _dbcontext.tokens.Include(a => a.account).Where(x => x.account == checkAccount).FirstOrDefault();
                if(checkAccount!= null && checkLogout != null)
                {
                    _dbcontext.tokens.RemoveRange(checkLogout);
                    if(await _dbcontext.SaveChangesAsync() > 0)
                    {
                        _userService.Logout();
                        return await Task.FromResult(new AccountDTO
                        {
                            username = "Success"
                        });
                    }
                }
            }catch(Exception ex)
            {
                return await Task.FromResult(new AccountDTO { username = $"{ex.Message}" });
            }

            return await Task.FromResult(new AccountDTO { username = "NUll" });
        }

        public async Task<TokenMessage> checkToken(string token)
        {
            string? tokens = null;

            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenreand = tokenhandler.ReadToken(token) as JwtSecurityToken;

            if(tokenreand != null)
            {
                // Lấy ra thời gian hết hạn của token
                var dateTokes = tokenreand.ValidTo;

                // Lấy ra thời gian hiện tại
                var date = DateTime.UtcNow;

                if(date > dateTokes)
                {
                    var checkClaim = _userService.name();

                    var checkAccount = _dbcontext.UserRegMsts.Include(r => r.role).Where(x => x.id == int.Parse(checkClaim)).FirstOrDefault();
                    var checkRole = _dbcontext.roles.Where(x => x.Equals(checkAccount.role)).FirstOrDefault();

                    var claim = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, checkAccount.username),
                        new Claim(JwtRegisteredClaimNames.Email, checkAccount.email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("Id", checkAccount.id.ToString()),
                        new Claim(ClaimTypes.Role, checkRole.name)
                    };
                    // Token hết hạn, Xóa khỏi database
                    var checkToken = _dbcontext.tokens.Where(x => x.token == token).FirstOrDefault();
                    if(checkToken != null)
                    {
                        _dbcontext.tokens.RemoveRange(checkToken);
                        await _dbcontext.SaveChangesAsync();
                        var AddToken = new Tokens
                        {
                            account = checkAccount,
                            token = GenerateToken(claim),
                            geneToken = GenerateRefreshToken()
                        };

                        _dbcontext.tokens.Add(AddToken);
                        await _dbcontext.SaveChangesAsync();

                        tokens = AddToken.token;

                        return await Task.FromResult(new TokenMessage
                        {
                            Message = "Token đã hết hạn. Vui lòng nhập Token mới",
                            Token = tokens
                        });
                    }
                }

            }

            tokens = "Token chưa hết hạn";
            return await Task.FromResult(new TokenMessage
            {
                Message = "Token chưa hết hạn",
                Token = tokens
            });
        }

        public async Task<PayLoad<List<AccountDTO>>> FindOne(IList<string> id)
        {
            string message = "";
            if (!id.Any() || id == null)
                return await Task.FromResult(PayLoad<List<AccountDTO>>.CreatedFail("Faild: No Item"));

            try
            {
                var datas = _dbcontext.UserRegMsts.Include(r => r.role).ToList();
                var list = new List<UserRegMst>();
                var listDTO = new List<AccountDTO>();
                for(var i = 0; i < id.Count; i++)
                {
                    
                    var data = id[i];
                    
                    bool checkChar = Regex.IsMatch(data, @"^\d+$"); // Câu lệnh kiểm tra xem có phải số không, "@"^\d+$"" đây là chuỗi ký tự để kiểm tra
                    if (int.TryParse(data, out int songuyen))
                    {
                        var dataInt = _dbcontext.UserRegMsts.Include(r => r.role).Where(x => x.id.Equals(songuyen)).FirstOrDefault();
                        if(dataInt != null)
                        {
                            list.Add(dataInt);
                        }
                    }

                    // Kiểm tra nếu không phải(mà là chữ) thì thực hiện lệnh trong "if"
                    if (!checkChar)
                    {
                        datas = datas.Where(x => x.username.Contains(data)).ToList();
                        if (datas.Count > 0 && datas.Any())
                        {
                            foreach (var item in datas)
                            {
                                list.Add(item);
                            }
                        }
                    }

                    // Nếu câu lệnh "string.IsNullOrWhiteSpace()" này mà bỏ dấu "!" này thì sẽ là kiểm tra xem chuỗi có khoảng trắng hoặc rỗng không
                    // Nếu để dấu "!" này cho câu lệnh "!string.IsNullOrWhiteSpace(data)" thì sẽ là kiểm tra dữ liệu chuyền vào có phải chuỗi không
                    //if (!string.IsNullOrWhiteSpace(data))
                    //{
                    //    datas = datas.Where(x => x.username.Contains(data)).ToList();
                    //    if(datas.Count > 0 && datas.Any())
                    //    {
                    //        foreach (var item in datas)
                    //        {
                    //            list.Add(item);
                    //        }
                    //    }
                        
                    //}

                    // Đây cũng là 1 cách để kiểm tra xem dữ liệu có phải chuỗi không
                    //if (data is string a)
                    //{

                    //}

                    // Đây là cách kiểm tra dữ liệu chuyền vào có phải ký tự không
                    //if (data is char)
                    //{

                    //}
                }

                foreach(var itemDTO in list)
                {
                    var accountDTOMapper = _mapper.Map<AccountDTO>(itemDTO);
                    var roles = _dbcontext.roles.Where(x => x.Equals(itemDTO.role)).FirstOrDefault();
                    if(roles != null)
                    {
                        accountDTOMapper.role_id = roles.id;
                    }
                    
                    listDTO.Add(accountDTOMapper);
                }

                return await Task.FromResult(PayLoad<List<AccountDTO>>.Successfully(listDTO));
                //return await Task.FromResult(PayLoad<>)
            }catch(Exception e)
            {
                message = e.Message;
            }

            return await Task.FromResult(PayLoad<List<AccountDTO>>.CreatedFail(message));
        }

        #region Role
        public async Task<PayLoad<object>> findAllRole()
        {
            string? message = null;
            try
            {
                var data = _dbcontext.roles.ToList();
                return await Task.FromResult(PayLoad<object>.Successfully(data));
            }catch(Exception e)
            {
                message = e.Message;
            }
            return await Task.FromResult(PayLoad<object>.CreatedFail(message));
        }
        #endregion
    }
}
