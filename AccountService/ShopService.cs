using AutoMapper;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using thuongmaidientus1.Common;
using thuongmaidientus1.EmailConfig;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.AccountService
{
    public class ShopService : IShopService
    {
        private readonly DBContexts _dbcontext;
        private readonly IMapper _mapper;
        private readonly EmailSetting _emaiSetting;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        public ShopService(DBContexts dbcontext, IMapper mapper, IOptions<EmailSetting> emailSetting, IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _emaiSetting = emailSetting.Value;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }
        public async Task<PayLoad<ShopDTO>> AddShop(ShopDTO shop) 
        {
            try
            {
                if (!shop.vanchuyens.Any() || shop.vanchuyens == null || shop.vanchuyens.Count <= 0)
                {
                    return await Task.FromResult(PayLoad<ShopDTO>.CreatedFail("Vận chuyển không được để trống!"));
                }
                var checkNameShop = _dbcontext.Inquirys.Where(x => x.Name == shop.Name).FirstOrDefault();
                if (checkNameShop != null)
                {
                    return await Task.FromResult(PayLoad<ShopDTO>.CreatedFail("Tên shop đã tồn tại"));
                }

                //var checkNameAccount = _dbcontext.tokens.Include(a => a.account).Where(x => x.token == shop.creator).FirstOrDefault();
                //if(checkNameAccount == null)
                //{
                //    return await Task.FromResult(PayLoad<ShopDTO>.CreatedFail("Token không tồn tại"));
                //}

                //var checkAccount = _dbcontext.UserRegMsts.Where(x => x.Equals(checkNameAccount.account)).FirstOrDefault();
                //if (checkAccount == null)
                //{
                //    return await Task.FromResult(PayLoad<ShopDTO>.CreatedFail("Tài khoản không tồn tại"));
                //}
                var checkAccount = checkIdOrToken(shop);
                var checkRole = _dbcontext.roles.Where(x => x.Equals(checkAccount.role)).FirstOrDefault();
                if (checkRole != null)
                {
                    shop.role = checkRole.name;
                }
                var MapShop = _mapper.Map<Inquiry>(shop);
                MapShop.account= checkAccount;
                MapShop.CretorEdit = checkAccount == null ? "Admin là người đã tạo và sửa Inquiry này" : "";
                StoneMst shopVanchuyen = new StoneMst();
                var sum = 0;
                foreach (var item in shop.vanchuyens)
                {
                    bool checkChuoi = Regex.IsMatch(item, @"^\d+$");
                    if (int.TryParse(item, out int songuyen))
                    {
                        var checkVanchuyen = _dbcontext.DimQltyMst.Where(x => x.id == songuyen).FirstOrDefault();
                        if (checkVanchuyen != null)
                        {
                            sum += 1;
                            var intCoverString = string.Format("{0}", sum);

                            shopVanchuyen.shop = MapShop;
                            shopVanchuyen.Vanchuyen = checkVanchuyen;
                            if (int.Parse(intCoverString) > sum - 1)
                            {
                                MapShop.vanchuyen = checkVanchuyen;
                                //MapShop.vanchuyen = shop.DimQltyMst[0] // "shop.DimQltyMst[0]": "[0]" nghĩa là lấy ra phần tử đầu tiên trong mảng;
                            }

                            // ĐÂY LÀ HÀM KIỂM TRA CÁC DỮ LIỆU VỪA BỊ THAY ĐỔI HOẶC VỪA ĐƯỢC ADD VÀO DATABASE, SỬ DỤNG "ChangeTracker"
                            //var changedEntries = _dbcontext.ChangeTracker.Entries() // Cách 1
                            //    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                            //    .ToList();

                            // Xử lý trước khi lưu
                            //var addedEntities = _dbcontext.ChangeTracker.Entries() // Cách 2
                            //    .Where(e => e.State == EntityState.Added)
                            //    .Select(e => e.Entity)
                            //    .ToList();

                            //foreach (var entry in changedEntries)
                            //{
                            //    // Kiểm tra các thay đổi ở đây
                            //    // entry.Entity chứa đối tượng được thay đổi hoặc thêm mới
                            //    // entry.State cho biết tình trạng thay đổi (Added hoặc Modified)

                            //if (entry.State == EntityState.Added)
                            //{
                            //    // Ghi log cho dữ liệu mới được thêm vào
                            //    Log("New entity added: " + entry.Entity.ToString());
                            //}

                            //else if (entry.State == EntityState.Modified)
                            //{
                            //    // Ghi log cho dữ liệu được chỉnh sửa
                            //    Log("Entity modified: " + entry.Entity.ToString());
                            //}
                            //}
                            //Console.WriteLine("Data: " + changedEntries);

                            _dbcontext.StoneMsts.Add(shopVanchuyen);
                        }
                    }

                    if (!checkChuoi)
                    {
                        var checkVanchuyens = _dbcontext.DimQltyMst.Where(x => x.name == item).FirstOrDefault();
                        if (checkVanchuyens != null)
                        {
                            sum += 1;
                            var intCoverString = string.Format("{0}", sum);
                            if (int.Parse(intCoverString) > sum - 1)
                            {
                                MapShop.vanchuyen = checkVanchuyens;
                            }
                            shopVanchuyen.shop = MapShop;
                            shopVanchuyen.Vanchuyen = checkVanchuyens;

                            _dbcontext.StoneMsts.Add(shopVanchuyen);
                        }
                    }

                }

                if (await _dbcontext.SaveChangesAsync() <= 0)
                {
                    return await Task.FromResult(PayLoad<ShopDTO>.CreatedFail("Error Add"));
                }
            }
            catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<ShopDTO>.CreatedFail(ex.Message));
            }


            return await Task.FromResult(PayLoad<ShopDTO>.Successfully(shop));


        }

        private UserRegMst checkIdOrToken(ShopDTO id)
        {
            //if(id.account_id != 0)
            //{
            //    var checkAccountID = _dbcontext.UserRegMsts.Include(r => r.role).Where(x => x.id == id.account_id).FirstOrDefault();
            //    if(checkAccountID.role.name != "Admin")
            //    {
            //        var checkRoleShop = _dbcontext.roles.Where(x => x.name == "Inquiry").FirstOrDefault();
            //        checkAccountID.role = checkRoleShop;
            //        _dbcontext.UserRegMsts.Update(checkAccountID);
            //        _dbcontext.SaveChanges();
            //        return checkAccountID;
            //    }
                
                
            //}

            //var checkTokenAccount = _dbcontext.tokens.Include(a => a.account).Where(x => x.token == id.creator).FirstOrDefault();
            //if(checkTokenAccount == null)
            //{
            //    return null;
            //}
            //var checkAccountByToken = _dbcontext.UserRegMsts.Include(r => r.role).Where(x => x.Equals(checkTokenAccount.account)).FirstOrDefault();
            //if( checkAccountByToken == null)
            //{
            //    return null;
            //}

            //if(checkAccountByToken.role.name != "Admin")
            //{
            //    var checkRoleShops = _dbcontext.roles.Where(x => x.name == "Inquiry").FirstOrDefault();
            //    checkAccountByToken.role = checkRoleShops;
            //    _dbcontext.UserRegMsts.Update(checkAccountByToken);
            //    _dbcontext.SaveChanges();
            //}


            //return checkAccountByToken;

            var checkToken = _dbcontext.tokens.Include(a => a.account).Where(x => x.token == id.creator).FirstOrDefault();
            if(checkToken != null)
            {
                var checkAccount = _dbcontext.UserRegMsts.Include(r => r.role).Where(x => x.Equals(checkToken.account)).FirstOrDefault();
                if(checkAccount != null)
                {
                    var checkRole = _dbcontext.roles.Where(x => x.name == "Inquiry").FirstOrDefault();
                    if (checkAccount.role.name == "Admin")
                    {
                        // Kiểm tra id được chuyền lên
                        var checkAccoutShop = _dbcontext.UserRegMsts.Include(rl => rl.role).Where(x => x.id == id.account_id).FirstOrDefault();
                        if(checkAccoutShop.role.name == "Admin")
                        {
                            return checkAccoutShop;
                        }
                        else if(checkAccoutShop.role.name != "Admin")
                        {
                            checkAccoutShop.role = checkRole;
                            _dbcontext.UserRegMsts.Update(checkAccoutShop);
                            _dbcontext.SaveChanges();

                            return checkAccoutShop;
                        }
                    }
                    else if(checkAccount.role.name != "Admin")
                    {
                        checkAccount.role = checkRole;
                        _dbcontext.UserRegMsts.Update(checkAccount);
                        _dbcontext.SaveChanges();
                        return checkAccount;
                    }
                }
            }

            return null;
        }

        public async Task<PayLoad<string>> DeleteShop(IList<string> id)
        {
            string message = "";
            if(!id.Any() || id == null)
            {
                return await Task.FromResult(PayLoad<string>.CreatedFail(message));
            }

            try
            {
                //var checkIdList = _dbcontext.Inquirys.Where(x => id.Contains(string.Format("{0}", x.id))).ToList();
                var checkIdList = _dbcontext.Inquirys.Where(x => id.Contains(x.id.ToString())).ToList();
                var listShop = new List<Inquiry>();
                if(checkIdList.Any() && checkIdList != null)
                {
                    foreach (var shop in checkIdList)
                    {
                        shop.Deleted = true;
                        listShop.Add(shop);
                    }
                }
                _dbcontext.Inquirys.UpdateRange(listShop);
                if(await _dbcontext.SaveChangesAsync() > 0) {
                    message = "Delete thành công";
                    return await Task.FromResult(PayLoad<string>.Successfully(message));
                }
                
            }catch(Exception ex)
            {
                message = ex.Message;
            }
            return await Task.FromResult(PayLoad<string>.CreatedFail(message));
        }

        public async Task<PayLoad<ShopDTO>> EditShop(int id, ShopDTO shop, string? username)
        {
            try
            {
                var checkIdShop = _dbcontext.Inquirys.Where(x => x.id == id).FirstOrDefault();
                if(checkIdShop == null)
                {
                    return await Task.FromResult(PayLoad<ShopDTO>.CreatedFail("Inquiry không tồn tại"));
                }

                var checkNameShop = _dbcontext.Inquirys.Where(x => x.Name != checkIdShop.Name && x.Name == shop.Name).FirstOrDefault();
                if(checkNameShop != null)
                {
                    return await Task.FromResult(PayLoad<ShopDTO>.CreatedFail("Tên Inquiry đã không tồn tại"));
                }

                var checAccount = _dbcontext.UserRegMsts.Where(x => x.username == username).FirstOrDefault();
                if(checAccount != null)
                {
                    checkIdShop.CretorEdit = checAccount.username + " là người đã chỉnh sửa bản ghi vào thời gian " + DateTimeOffset.UtcNow;
                }
                else
                {
                    checkIdShop.CretorEdit = "Chưa có người sửa";
                }
                var checkAccount = checkIdOrToken(shop);
                var checkRole = _dbcontext.roles.Where(x => x.name == checkAccount.role.name).FirstOrDefault();
                if (checkRole != null)
                {
                    shop.role = checkRole.name;
                }
                checkIdShop.Name = shop.Name;
                checkIdShop.UpdatedAt = DateTimeOffset.UtcNow;
                checkIdShop.diachi = shop.diachi;
                checkIdShop.email = shop.email;
                checkIdShop.image = shop.image;
                checkIdShop.sodienthoai = shop.sodienthoai;
                checkIdShop.account = checkAccount;
              
                

                var checkShopVanChuyen = _dbcontext.StoneMsts.Include(s => s.shop).Where(x => x.shop == checkIdShop).ToList();
                if(checkShopVanChuyen.Count > 0)
                {
                    _dbcontext.StoneMsts.RemoveRange(checkShopVanChuyen);
                    if(await _dbcontext.SaveChangesAsync() <= 0)
                    {
                        return await Task.FromResult(PayLoad<ShopDTO>.CreatedFail("Error Add"));
                    }
                }

                var listVanChuyenShop = new List<StoneMst>();
                var sum = 0;

                for(var i = 0; i < shop.vanchuyens.LongCount(); i++)
                {
                    var data = shop.vanchuyens[i];
                    bool checkInt = Regex.IsMatch(data, @"^\d+$");
                    if(int.TryParse(data, out int songuyen))
                    {
                        var checkId = _dbcontext.DimQltyMst.Where(x => x.id == songuyen).FirstOrDefault();
                        if(checkId != null)
                        {
                            sum += 1;
                            if(sum == 1)
                            {
                                checkIdShop.vanchuyen = checkId;
                            }
                            listVanChuyenShop.Add(new StoneMst
                            {
                                shop = checkIdShop,
                                Vanchuyen = checkId
                            });
                        }
                    }
                    else if (!checkInt)
                    {
                        var checkName = _dbcontext.DimQltyMst.Where(x => x.name == data).FirstOrDefault();
                        if (checkName != null)
                        {
                            listVanChuyenShop.Add(new StoneMst
                            {
                                shop = checkIdShop,
                                Vanchuyen = checkName
                            });
                        }
                    }
                }

                _dbcontext.StoneMsts.AddRange(listVanChuyenShop);
                _dbcontext.Inquirys.UpdateRange(checkIdShop);

                if(await _dbcontext.SaveChangesAsync() > 0)
                {
                    return await Task.FromResult(PayLoad<ShopDTO>.Successfully(shop));
                }

            }catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<ShopDTO>.CreatedFail(ex.Message));
            }

            return await Task.FromResult(PayLoad<ShopDTO>.CreatedFail("Error Add"));
        }

        public async Task<PayLoad<object>> FindAll(string? name, int page = 1, int pageSize = 20)
        {
            try
            {
                page = page <= 0 ? 1 : page;
                pageSize = pageSize < 0 ? 10 : pageSize;
                var data = _dbcontext.Inquirys.Include(a => a.account).Include(v => v.vanchuyen).Where(x => !x.Deleted).Select(x => new
                {
                    name = x.Name,
                    diachi = x.diachi,
                    email = x.email,
                    sodienthoai = x.sodienthoai,
                    image = x.image,
                    account = x.account.username != null ? x.account.username + " là người đã tạo shop và lúc " + x.CreatedAt : "Chưa có người dùng tạo shop",
                    vanchuyen = x.vanchuyen.name != null ? x.vanchuyen.name : "Chưa có vận chuyển",
                    id = x.id

                }).AsQueryable();
                if (!string.IsNullOrEmpty(name))
                    data = data.Where(x => x.name.Contains(name)).AsQueryable();

                var pageList = await PageList<object>.Create(data, page, pageSize);

                return await Task.FromResult(PayLoad<object>.Successfully(new
                {
                    data = pageList,
                    page,
                    pageList.pageSize,
                    pageList.totalCounts,
                    pageList.totalPages
                }));

            }catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<object>.Successfully(new
                {
                    name = ex.Message
                }));
            }

            
        }

        public async Task<PayLoad<object>> FindOneIdOrName(IList<string> name)
        {
            try
            {
                if(name == null || !name.Any() || name.Count <= 0)
                    return await Task.FromResult(PayLoad<object>.CreatedFail("Dữ liệu null"));

                var list = new List<object>();
                for(var i = 0; i < name.Count; i++)
                {
                    var data = name[i];
                    bool checkInt = Regex.IsMatch(data, @"^\d+$");
                    if(int.TryParse(data, out int songuyen))
                    {
                        var checkId = _dbcontext.Inquirys.Include(svc => svc.ShopVanchuyens)
                                .ThenInclude(sv => sv.shop).Include(a => a.account).Include(p => p.Products).Include(v => v.vanchuyen)
                            .Where(x => x.id == songuyen).Select(x => new
                        {
                            name = x.Name,
                            diachi = x.diachi,
                            email = x.email,
                            sodienthoai = x.sodienthoai,
                            image = x.image,
                            account_id = x.account != null ? x.account.id : 0,
                            vanchuyen = x.vanchuyen != null ? x.vanchuyen.id : 0,
                            vanchuyens = x.ShopVanchuyens.Where(a => a.shop.id == x.id && a.shop.id != 0).Select(se => new
                            {
                                id = se.id,
                                shop_id = se.shop.id,
                                vanchuyen_id = se.Vanchuyen.id,
                                shop_name = se.shop.Name,
                                vanchuyen_nam = se.Vanchuyen.name
                            }).ToList(),
                            products = x.Products.Where(a => !string.IsNullOrEmpty(a.title) || a.Shops == x).Select(vc => vc.id).ToList(),
                            id = x.id
                        }).SingleOrDefault();
                        if(checkId != null)
                        {
                            list.Add(checkId);
                        }
                    }
                    if (!checkInt)
                    {
                        var checkName = _dbcontext.Inquirys.Where(x => x.Name.Contains(data)).ToList();
                        if(checkName != null)
                        {
                            list.Add(checkName); 
                        }
                    }
                }

                return await Task.FromResult(PayLoad<object>.Successfully(list));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<object>.CreatedFail(ex.Message));
            }
        }
    }
}
