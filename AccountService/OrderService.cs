using AutoMapper;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using thuongmaidientus1.Common;
using thuongmaidientus1.EmailConfig;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.AccountService
{

    public class OrderService : IOrderService
    {
        private readonly DBContexts _dbcontext;
        private readonly IMapper _mapper;
        private readonly EmailSetting _emaiSetting;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly Jwt _jwt;
        private readonly IUserService _userService;
        public OrderService(DBContexts dbcontext, IMapper mapper, IOptions<EmailSetting> emailSetting, IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IOptionsMonitor<Jwt> jwt, IUserService userService)
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

        public async Task<PayLoad<XulydonhangDTO>> AddDonHangXuLy(IList<XulydonhangDTO> xulydonhangs)
        {
            try
            {
                var listXulydonhang = new List<JewelTypeMst>();
                if (!xulydonhangs.Any() || xulydonhangs.Count <= 0)
                    return await Task.FromResult(PayLoad<XulydonhangDTO>.CreatedFail("Dữ liệu Null"));

                for(var i = 0; i <  xulydonhangs.Count; i++)
                {
                    var data = xulydonhangs[i];
                    var checkVanchuyen = _dbcontext.DimQltyMst.Where(x => x.id == data.vanchuyen_id).FirstOrDefault();

                    var checkDonhang = _dbcontext.JewelTypeMst.Where(x => x.product_id == data.product_id && !x.Deleted).FirstOrDefault();
                    if(checkDonhang != null)
                    {
                        if(data.soluong == 0)
                        {
                            checkDonhang.soluong += 1;
                        }
                        else
                        {
                            checkDonhang.soluong += data.soluong;
                        }
                        if (checkVanchuyen != null)
                        {
                            checkDonhang.vanchuyen = checkVanchuyen;
                        }

                        checkDonhang.total = (int)(checkDonhang.soluong * checkDonhang.price.Value); // Nhân số nguyên với số thực và trả về số nguyên
                        _dbcontext.JewelTypeMst.Update(checkDonhang);
                    }
                    else
                    {
                        var checkIdProduct = _dbcontext.ProdMsts.Where(x => x.id == data.product_id && !x.Deleted).FirstOrDefault();
                        var checkAccount = _dbcontext.UserRegMsts.Where(x => x.id == data.account_id && !x.Deleted).FirstOrDefault();
                        if (checkIdProduct != null && checkAccount != null)
                        {
                            
                            var MapXuLyDonHang = _mapper.Map<JewelTypeMst>(data);
                            MapXuLyDonHang.Account = checkAccount;
                            MapXuLyDonHang.trangthai = data.trangthai != null ? data.trangthai.ToString() : "Pending";
                            //if(data.trangthai != null /*|| data.trangthai != default(Status)*/) // "data.trangthai != default(Status)" kiểm tra giá trị mặc định của "enum"
                            //{

                            //}
                            if (checkVanchuyen != null)
                            {
                                MapXuLyDonHang.vanchuyen = checkVanchuyen;
                            }
                            listXulydonhang.Add(MapXuLyDonHang);
                        }
                    }
                }

                _dbcontext.JewelTypeMst.AddRange(listXulydonhang);
                if(await _dbcontext.SaveChangesAsync() > 0)
                {
                    return await Task.FromResult(PayLoad<XulydonhangDTO>.Successfully(new XulydonhangDTO
                    {
                        id = xulydonhangs[0].id,
                        account_id = xulydonhangs[0].account_id,
                        price = xulydonhangs[0].price,
                        product_id = xulydonhangs[0].product_id,
                        product_name = xulydonhangs[0].product_name,
                        soluong = xulydonhangs[0].soluong,
                        total = xulydonhangs[0].total,
                        trangthai = xulydonhangs[0].trangthai
                    }));
                }

                return await Task.FromResult(PayLoad<XulydonhangDTO>.CreatedFail("Add Faild"));
            }catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<XulydonhangDTO>.CreatedFail(ex.Message));
            }
        }

        public async Task<PayLoad<CertifyMst>> AddOrder(string? name, IList<XulydonhangDTO> donhang)
        {
            try {
                if (!donhang.Any() || donhang.Count <= 0)
                    return await Task.FromResult(PayLoad<CertifyMst>.CreatedFail("Không có data"));

                var checkAccountName = _dbcontext.UserRegMsts.Where(x => x.username == name && !x.Deleted).FirstOrDefault();
                if(checkAccountName == null)
                {
                    return await Task.FromResult(PayLoad<CertifyMst>.CreatedFail("UserRegMst không tồn tại"));
                }
                var order = new CertifyMst()
                {
                    Accounts = checkAccountName,
                    Deleted = false,
                    orderName = "Đơn hàng được mua bởi khách hàng " + checkAccountName.username + " vào lúc " + DateTimeOffset.UtcNow,
                    status = "Done"
                };
                _dbcontext.CertifyMsts.Add(order);
                if(await _dbcontext.SaveChangesAsync() > 0)
                {
                    var orderId = order;
                    var listOrderLine = new List<DimMst>();
                    for(var i = 0;  i < donhang.Count; i++)
                    {
                        var data = donhang[i];
                        var checkProduct = _dbcontext.ProdMsts.Where(x => x.id == data.product_id && !x.Deleted).FirstOrDefault();
                        if(checkProduct != null)
                        {
                            var orderLine = new DimMst()
                            {
                                Orders = orderId,
                                price = data.price,
                                Products = checkProduct,
                                quantity = data.soluong,
                                total = data.total
                            };

                            listOrderLine.Add(orderLine);
                        }
                    }
                       
                    _dbcontext.DimMsts.AddRange(listOrderLine);
                    if(await _dbcontext.SaveChangesAsync() > 0)
                    {
                        return await Task.FromResult(PayLoad<CertifyMst>.Successfully(order));
                    }

                    
                }
                return await Task.FromResult(PayLoad<CertifyMst>.CreatedFail("Add Faild"));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<CertifyMst>.CreatedFail(ex.Message));
            }
        }

        public async Task<PayLoad<string>> DeleteOrder(IList<string> id)
        {
            string? message = null;
            try
            {
                if(!id.Any() || id.Count <= 0)
                    return await Task.FromResult(PayLoad<string>.CreatedFail("Data Null"));

                for(var i = 0; i <  id.Count; i++)
                {
                    var data = id[i];
                    bool checkInt = Regex.IsMatch(data, @"^\d+$");
                    if(int.TryParse(data, out int songuyen))
                    {
                        var checkId = _dbcontext.DimMsts.Include(o => o.Orders).Where(x => x.Orders.id == songuyen).ToList();
                        var checkOrderId = _dbcontext.CertifyMsts.Where(x => x.id == songuyen).FirstOrDefault();
                        if(checkId.Count > 0)
                        {
                            foreach(var item in checkId)
                            {
                                item.Deleted = true;
                                _dbcontext.DimMsts.Update(item);
                            }
                        }
                        if(checkOrderId != null)
                        {
                            checkOrderId.Deleted = true;
                            _dbcontext.CertifyMsts.Update(checkOrderId);
                        }
                        
                    }
                    if(!checkInt)
                    {
                        var checkId = _dbcontext.DimMsts.Include(o => o.Orders).Where(x => x.Orders.orderName == data).ToList();
                        var checkOrderId = _dbcontext.CertifyMsts.Where(x => x.orderName == data).FirstOrDefault();
                        if (checkId.Count > 0)
                        {
                            foreach (var item in checkId)
                            {
                                item.Deleted = true;
                                _dbcontext.DimMsts.Update(item);
                            }
                        }
                        if (checkOrderId != null)
                        {
                            checkOrderId.Deleted = true;
                            _dbcontext.CertifyMsts.Update(checkOrderId);
                        }
                    }
                }

                if(await _dbcontext.SaveChangesAsync() > 0)
                {
                    message = "Success";
                    return await Task.FromResult(PayLoad<string>.Successfully(message));
                }

                return await Task.FromResult(PayLoad<string>.CreatedFail("Delete Faild"));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<string>.CreatedFail(ex.Message));
            }
        }

        public async Task<PayLoad<object>> FindAll(string? name, int page = 1, int pageSize = 10)
        {
            try
            {
                IQueryable<object>? checkOrderLine;
                page = page <= 0 ? 1 : page;
                pageSize = pageSize <= 0 ? 10 : pageSize;
                if(int.TryParse(name, out int songuyen))
                {
                    checkOrderLine = _dbcontext.DimMsts.Include(o => o.Orders).Include(p => p.Products).ThenInclude(s => s.Shops).ThenInclude(a => a.account).Where(x => x.Products.Shops.account.id == songuyen && !x.Deleted).Select(x => new
                    {
                        Id = x.id,
                        Product_Name = x.Products.title,
                        Order_Name = x.Orders.orderName,
                        Price = x.price,
                        Soluong = x.quantity,
                        Total = x.total
                    }).AsQueryable();
                }
                else
                {
                    checkOrderLine = _dbcontext.DimMsts.Include(o => o.Orders).Include(p => p.Products).ThenInclude(s => s.Shops).ThenInclude(a => a.account).Where(x => x.Products.Shops.account.username == name && !x.Deleted).Select(x => new
                    {
                        Id = x.id,
                        Product_Name = x.Products.title,
                        Order_Name = x.Orders.orderName,
                        Price = x.price,
                        Soluong = x.quantity,
                        Total = x.total
                    }).AsQueryable();
                }
                //var checkOrderLine = _dbcontext.DimMst.Include(o => o.Orders).Include(p => p.Products).ThenInclude(s => s.Shops).ThenInclude(a => a.account).Where(x => x.Products.Shops.account.username == name && !x.Deleted).Select(x => new
                //{
                //    Id = x.id,
                //    Product_Name = x.Products.title,
                //    Order_Name = x.Orders.orderName,
                //    Price = x.price,
                //    Soluong = x.quantity,
                //    Total = x.total
                //}).AsQueryable();

                var PageList = await PageList<object>.Create(checkOrderLine, page, pageSize);

                return await Task.FromResult(PayLoad<object>.Successfully(new
                {
                    data = PageList,
                    page,
                    PageList.pageSize,
                    PageList.totalCounts,
                    PageList.totalPages
                }));
                
                
            }catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<object>.CreatedFail(ex.Message));
            }
        }

        public async Task<PayLoad<object>> FindAllAccountDonHangByMax(string? name, string? account_name, int page = 1, int pageSize = 20)
        {
            try
            {
                page = page <= 0 ? 1 : page;
                pageSize = pageSize <= 0 ? 20 : pageSize;
                if (account_name == null)
                {
                    return await Task.FromResult(PayLoad<object>.CreatedFail("Dữ liệu null"));
                }
                var checkAccount = _dbcontext.UserRegMsts.Include(r => r.role).Where(x => x.username == account_name).FirstOrDefault();
                if (checkAccount == null)
                {
                    return await Task.FromResult(PayLoad<object>.CreatedFail("Dữ liệu null"));
                }
                /*
                    GIẢI THÍCH ĐOẠN CODE NÀY
                    "Include(a => a.Accounts) và .Include(o => o.DimMst).ThenInclude(p => p.Products).ThenInclude(sh => sh.Shops)": Đoạn mã này
                    sử dụng Include để tải các thông tin liên quan từ cơ sở dữ liệu. Nó sẽ load 
                    thông tin của "Accounts, DimMst, Products, và Shops" khi truy vấn 
                    được thực hiện, giúp truy xuất thông tin một cách thuận tiện hơn.
                 */
                var data = _dbcontext.CertifyMsts
                    .Include(a => a.Accounts)
                    .Include(o => o.OrderDetails).ThenInclude(p => p.Products).ThenInclude(sh => sh.Shops)
                    /*
                        ".SelectMany(order => order.DimMst, (order, orderDetail) => new { ... })": "SelectMany" được sử dụng
                        để kết hợp thông tin từ Orders và DimMst. Mỗi CertifyMst sẽ được kết hợp với
                        một hoặc nhiều "OrderDetail". Trong biểu thức "lambda", chúng ta tạo một
                        đối tượng mới chứa thông tin như "Account_id, ProductId, OrderId, Quantity, UserName, và ShopName" bằng cách
                        trích xuất thông tin từ các đối tượng CertifyMst và OrderDetail.

                        Trong SelectMany, tham số đầu tiên order đại diện cho mỗi đối tượng Orders. 
                        Tham số thứ hai orderDetail đại diện cho mỗi đối tượng trong "DimMst" 
                        của từng "CertifyMst". Bằng cách này, chúng ta có thể truy cập thông tin từ cả
                        "Orders và DimMst" để kết hợp chúng lại với nhau.
                     */
                    .SelectMany(order => order.OrderDetails, (order, orderDetail) => new
                    {
                        Account_id = order.Accounts.id,
                        ProductId = orderDetail.Products.id,
                        ProductId_name = orderDetail.Products.title,
                        OrderId = orderDetail.Orders.id,
                        Quantity = orderDetail.quantity,
                        UserName = order.Accounts.username,
                        ShopName = orderDetail.Products.Shops.Name,
                        ShopId = orderDetail.Products.Shops.id,
                        AccountImage = order.Accounts.image
                    })
                    /* ".GroupBy(x => x.Account_id):" Sau khi tạo các đối tượng mới, chúng ta sử dụng
                     GroupBy để nhóm chúng theo Account_id. Điều này tạo ra các nhóm dựa trên 
                     Account_id và thông tin liên quan.
                    */
                    .GroupBy(x => x.Account_id)
                    /*
                        ".Select(group => new { ... }):" Trong phần này, chúng ta sử dụng "Select" để 
                        chọn ra thông tin từ mỗi nhóm. MostPurchasedProduct là sản phẩm được mua 
                        nhiều nhất trong mỗi nhóm dựa trên "Quantity", "Soluong" là tổng số lượng sản phẩm
                        mua trong mỗi nhóm, sử dụng các hàm như "OrderByDescending" để sắp xếp các
                        bản ghi và "Sum" để tính toán thông tin số lượng.
                     */
                    .Select(group => new
                    {
                        // Biến "group" này sẽ lấy hết dữ liệu trong "SelectMany()" ở phía trên rồi hiển thị lại dữ liệu ở đây
                        Account_id = group.Key,
                        //MostPurchasedProduct = group.OrderByDescending(item => item.Quantity).ToList(), // Lấy ra 1 List sản phẩm được mua nhiều nhất của khách hàng
                        MostPurchasedProduct = group.OrderByDescending(item => item.Quantity).FirstOrDefault(), // Lấy ra 1 sản phẩm được mua nhiều nhất của khách hàng
                        Soluong = group.Sum(item => item.Quantity),
                        Name = group.Select(item => item.UserName).FirstOrDefault(),
                        Shop_name = group.Select(item => item.ShopName).FirstOrDefault(),
                        ProductId_name = group.Select(item => item.ProductId_name).FirstOrDefault(),
                        Shop_id = group.Select(item => item.ShopId).FirstOrDefault(),
                        Account_image = group.Select(item => item.AccountImage).FirstOrDefault()
                    })
                    /* ".OrderByDescending(orderBy => orderBy.Soluong):" Cuối cùng, chúng ta sắp xếp
                    kết quả theo "Soluong" (tổng số lượng sản phẩm) giảm dần để lấy ra người mua 
                    nhiều nhất. */
                    .OrderByDescending(orderBy => orderBy.Soluong)
                    .ToList();

                if (!string.IsNullOrEmpty(name))
                    data = data.Where(x => x.Name == name).ToList();

                
                if(checkAccount.role.name == "Inquiry")
                {
                    var checkShop = _dbcontext.Inquirys.Include(a => a.account).Where(x => x.account.Equals(checkAccount)).FirstOrDefault();
                    if(checkShop == null)
                    {
                        return await Task.FromResult(PayLoad<object>.CreatedFail("Inquiry Null"));
                       
                    }
                    data = data.Where(x => x.Shop_id == checkShop.id).ToList();



                }
                var pageList = new PageList<object>(data, page - 1, pageSize);
                /* 
                   Trả về danh sách chứa thông tin về người mua nhiều sản phẩm nhất, bao gồm thông 
                   tin về sản phẩm được mua nhiều nhất và tổng số lượng sản phẩm mua của mỗi người.
                 */
                return await Task.FromResult(PayLoad<object>.Successfully(new
                {
                    data = pageList,
                    page,
                    pageList.pageSize,
                    pageList.totalCounts,
                    pageList.totalPages
                }));


            }
            catch (Exception ex)
            {
                return await Task.FromResult(PayLoad<object>.CreatedFail(ex.Message));
            }
        }

        public async Task<PayLoad<object>> FindAllDonHangByMax(string? name, string? account_name, int page = 1, int pageSize = 20)
        {
            try
            {
                page = page <= 0 ? 1 : page;
                pageSize = pageSize <= 0 ? 20 : pageSize;
                if (account_name == null)
                {
                    return await Task.FromResult(PayLoad<object>.CreatedFail("Dữ liệu null"));
                }
                var checkAccount = _dbcontext.UserRegMsts.Include(r => r.role).Where(x => x.username == account_name).FirstOrDefault();
                if(checkAccount == null)
                {
                    return await Task.FromResult(PayLoad<object>.CreatedFail("Dữ liệu null"));
                }

                // Câu lệnh này là tính số lượng bản ghi được mua nhiều nhất
                var data = _dbcontext.DimMsts
                    .Include(p => p.Products)
                    .ThenInclude(s => s.Shops)
                    .Include(o => o.Orders)
                    .ThenInclude(a => a.Accounts)
                    .GroupBy(p => new { p.Products.id, p.Products.title }) // Sử dụng Group by để gom nhóm,  Group by ProductId và Title của Products
                    .Select(s => new
                    {
                        ProductId = s.Key.id,
                        ProductName = s.Key.title,
                        Soluong = s.Sum(x => x.quantity),// Sử dụng hàm "Sum()" để tính tổng số lượng được mua của bản ghi đấy
                                                         // Cách 1: Để lấy ra thông tin của bảng CertifyMst và lấy ra bảng UserRegMst trong bảng CertifyMst
                                                         //Name = s.Select(od => od.Orders.Accounts.username).ToList(), // Lấy ra 1 List danh sách UserRegMst

                        // Cách 2: Để lấy ra thông tin của bảng CertifyMst và lấy ra bảng UserRegMst trong bảng CertifyMst
                        //Name = s.Select(od => od.Orders.Accounts.username) // Lấy ra 1 List danh sách UserRegMst

                        // Cách 3: Để lấy ra thông tin của bảng CertifyMst và lấy ra bảng UserRegMst trong bảng CertifyMst
                        Name = s.Select(od => od.Orders.Accounts.username).FirstOrDefault(), // Lấy ra 1 UserRegMst sử dụng hàm "FirstOrDefault()"
                        Shop_name = s.Select(shop => shop.Products.Shops.Name).FirstOrDefault(),
                        Account_image = s.Select(image => image.Orders.Accounts.image).FirstOrDefault()
                    })
                    .OrderByDescending(x => x.Soluong)
                    .ToList(); // Execute the query and retrieve the result as a list

                if (!string.IsNullOrEmpty(name))
                    data = data.Where(x => x.Name == name).ToList();

                if (checkAccount.role.name == "Admin")
                {
                    
                    
                    var pageList = new PageList<object>(data, page - 1, pageSize);
                    return await Task.FromResult(PayLoad<object>.Successfully(new
                    {
                        data = pageList,
                        page,
                        pageList.pageSize,
                        pageList.totalCounts,
                        pageList.totalPages
                    }));
                }
                else if(checkAccount.role.name == "Inquiry")
                {
                    var checkShop = _dbcontext.Inquirys.Include(a => a.account).Where(x => x.account.Equals(checkAccount)).FirstOrDefault();
                    if(checkShop != null)
                    {
                        data = data.Where(x => x.Shop_name == checkShop.Name).ToList();
                        var pageList = new PageList<object>(data, page - 1, pageSize);
                        return await Task.FromResult(PayLoad<object>.Successfully(new
                        {
                            data = pageList,
                            page,
                            pageList.pageSize,
                            pageList.totalCounts,
                            pageList.totalPages
                        }));
                    }
                }
                return await Task.FromResult(PayLoad<object>.CreatedFail("Bạn không có quyền"));


            }
            catch(Exception e)
            {
                return await Task.FromResult(PayLoad<object>.CreatedFail(e.Message));
            }
        }

        public async Task<PayLoad<object>> FindAllDonHangXuLy(string? name, int page = 1, int pageSize = 20)
        {
            try
            {
                UserRegMst? checkAccount = null;
                page = page <= 0 ? 1 : page;
                pageSize = pageSize <= 0 ? 20 : pageSize;
                var listData = new List<object>(); // Sử dụng "List<object>()" để Add 1 list danh sách dạng "AsQueryable()" vào trong List
                if(int.TryParse(name, out int songuyen))
                {
                    checkAccount = _dbcontext.UserRegMsts.Include(r => r.role).Where(x => x.id == songuyen).FirstOrDefault();
                }
                else
                {
                    checkAccount = _dbcontext.UserRegMsts.Include(r => r.role).Where(x => x.username == name).FirstOrDefault();
                }
                if (checkAccount != null)
                {
                    var data = _dbcontext.JewelTypeMst.Include(r => r.Account).Include(v => v.vanchuyen).Where(x => !x.Deleted).Select(xs => new
                    {
                        id = xs.id,
                        product_id = xs.product_id,
                        product_name = xs.product_name,
                        price = xs.price,
                        soluong = xs.soluong,
                        total = xs.total,
                        trangthai = xs.trangthai,
                        account = xs.Account.username,
                        vanchuyen = xs.vanchuyen.name
                    }).AsQueryable()/*.ToList()*/; // Để ".ToList()" sau "AsQueryable()" để có thể sử dụng vòng lặp "for()"
                    if (checkAccount.role.name == "Admin")
                    {
                        var pageList = await PageList<object>.Create(data, page, pageSize);
                        return await Task.FromResult(PayLoad<object>.Successfully(new
                        {
                            data = pageList,
                            page,
                            pageList.pageSize,
                            pageList.totalCounts,
                            pageList.totalPages
                        }));
                    }
                    else if(checkAccount.role.name == "Inquiry")
                    {
                        
                        var checkShop = _dbcontext.Inquirys.Include(a => a.account).Where(x => x.account.Equals(checkAccount)).FirstOrDefault();
                        if(checkShop != null)
                        {
                            // Cách 1
                            //foreach (var item in data)
                            //{
                            //    var checkProduct = _dbcontext.ProdMsts.Include(s => s.Shops).Include(a => a.Accounts).Where(x => x.id == item.product_id && x.Shops.Equals(checkShop)).FirstOrDefault();
                            //    if (checkProduct != null)
                            //    {
                            //        listData.Add(item);
                            //    }

                            //}

                            // Cách 2
                            var checkProdut = _dbcontext.ProdMsts.Include(s => s.Shops).Include(a => a.Accounts).Where(x => x.Shops.Equals(checkShop) && !x.Deleted).ToList();
                            for(var i = 0; i < checkProdut.Count; i++)
                            {
                                var dataItem = checkProdut[i];
                                var checkChoXacNhan = _dbcontext.JewelTypeMst.Include(a => a.Account).Include(v => v.vanchuyen).Where(x => x.product_id == dataItem.id && !x.Deleted).Select(d => new
                                {
                                    id = d.id,
                                    product_id = d.product_id,
                                    product_name = d.product_name,
                                    price = d.price,
                                    soluong = d.soluong,
                                    total = d.total,
                                    trangthai = d.trangthai,
                                    account = d.Account.username,
                                    vanchuyen = d.vanchuyen.name
                                }).FirstOrDefault();
                                if( checkChoXacNhan != null)
                                {
                                    listData.Add(checkChoXacNhan);
                                }
                            }

                            var PageList = new PageList<object>(listData, page - 1, pageSize);
                            return await Task.FromResult(PayLoad<object>.Successfully(new
                            {
                                data = PageList,
                                page,
                                PageList.pageSize,
                                PageList.totalCounts,
                                PageList.totalPages
                            }));
                        }
                        
                    }
                    else if(checkAccount.role.name == "User")
                    {
                        //var CheckXuLyDonHangUser = _dbcontext.JewelTypeMst.Include(a => a.UserRegMst).Where(x => x.UserRegMst.Equals(checkAccount)).Select(xs => new
                        //{
                        //    id = xs.id,
                        //    product_id = xs.product_id,
                        //    product_name = xs.product_name,
                        //    price = xs.price,
                        //    soluong = xs.soluong,
                        //    total = xs.total,
                        //    trangthai = xs.trangthai,
                        //    account = xs.UserRegMst.username
                        //}).ToList();
                        data = data.Where(x => x.account == checkAccount.username).AsQueryable();
                        listData.AddRange(data);
                        var PageList = new PageList<object>(listData, page - 1, pageSize);
                        return await Task.FromResult(PayLoad<object>.Successfully(new
                        {
                            data = PageList,
                            page,
                            PageList.pageSize,
                            PageList.totalCounts,
                            PageList.totalPages
                        }));
                    }
                }
                return await Task.FromResult(PayLoad<object>.CreatedFail("Faild"));
            }catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<object>.CreatedFail(ex.Message));
            }
        }

        public async Task<PayLoad<object>> FindAllShopDonHangByMax(string? name, string? account_name, int page = 1, int pageSize = 20)
        {
            try
            {
                bool checkInt = Regex.IsMatch(account_name, @"^\d+$");
                UserRegMst? account = null;
                if(int.TryParse(account_name, out int songuyen))
                {
                    account = _dbcontext.UserRegMsts.Where(x => x.id == songuyen).FirstOrDefault();
                }
                if (!checkInt)
                {
                    account = _dbcontext.UserRegMsts.Where(x => x.username == account_name).FirstOrDefault();
                }

                var data = _dbcontext.Inquirys
                    .SelectMany(s => s.Products.SelectMany(p => p.OrderDetails), (s, od) => new
                    {
                        ShopId = s.id,
                        ShopName = s.Name,
                        Quantity = od.quantity,
                        product = od.Products.title,
                        ShopImage = s.image,
                        total = od.total
                    }).ToList()
                    .GroupBy(x => new { x.ShopId, x.ShopName })
                    .Select(g => new
                    {
                        ListShopProduct = g.OrderByDescending(item => item.Quantity),
                        ShopId = g.Key.ShopId,
                        ShopName = g.Key.ShopName,
                        TotalQuantity = g.Sum(x => x.Quantity),
                        ShopImage = g.Select(x => x.ShopImage),
                        Total = g.Sum(x => x.total)
                    })
                    .OrderByDescending(x => x.TotalQuantity)
                    .Take(10) // Lấy ra 10 cửa hàng bán được nhiều đơn hàng nhất
                    .ToList();

                var pageList = new PageList<object>(data, page - 1, pageSize);
                return await Task.FromResult(PayLoad<object>.Successfully(new
                {
                    data = pageList,
                    page,
                    pageList.pageSize,
                    pageList.totalCounts,
                    pageList.totalPages
                }));
                    

            }catch(Exception e)
            {
                return await Task.FromResult(PayLoad<object>.CreatedFail(e.Message));
            }
        }

        public async Task<PayLoad<object>> FindAllShopProductMax(string? name, string? account_name, int page = 1, int pageSize = 20)
        {
            try
            {
                bool checkInt = Regex.IsMatch(account_name, @"^\d+$");
                UserRegMst? account = null;
                if (int.TryParse(account_name, out int songuyen))
                {
                    account = _dbcontext.UserRegMsts.Where(x => x.id == songuyen).FirstOrDefault();
                }
                if (!checkInt)
                {
                    account = _dbcontext.UserRegMsts.Where(x => x.username == account_name).FirstOrDefault();
                }

                var data = _dbcontext.Inquirys
                    .SelectMany(s => s.Products, (s, od) => new
                    {
                        ShopId = s.id,
                        ShopName = s.Name,
                        ProductId = od.id,
                        productName = od.title,
                        shopImage = s.image
                    })
                    .GroupBy(x => new { x.ShopId, x.ShopName })
                    .Select(g => new
                    {
                        ListShopProduct = g.OrderByDescending(item => item.ShopId).FirstOrDefault(),
                        ShopId = g.Key.ShopId,
                        ShopName = g.Key.ShopName,
                        TotalQuantity = g.Count(),
                        shopImage = g.Select(x => x.shopImage)
                    })
                    .OrderByDescending(x => x.TotalQuantity)
                    .Take(10) // Lấy ra 10 cửa hàng bán được nhiều đơn hàng nhất
                    .ToList();

                var pageList = new PageList<object>(data, page - 1, pageSize);
                return await Task.FromResult(PayLoad<object>.Successfully(new
                {
                    data = pageList,
                    page,
                    pageList.pageSize,
                    pageList.totalCounts,
                    pageList.totalPages
                }));


            }
            catch (Exception e)
            {
                return await Task.FromResult(PayLoad<object>.CreatedFail(e.Message));
            }
        }

        public async Task<PayLoad<XulydonhangDTO>> UpdateDonHangXuLy(int product_id, string? name, string? account_name, Status? status = Status.Pending)
        {
            try
            {
                var checkProduct = _dbcontext.JewelTypeMst.Include(a => a.Account).Where(x => x.product_id == product_id && !x.Deleted).FirstOrDefault();
                var checkAccount = _dbcontext.UserRegMsts.Where(x => x.username == name && !x.Deleted).FirstOrDefault();
                if(checkProduct != null && checkAccount != null)
                {
                    if(status == Status.Delete)
                    {
                        var checkXuLyDH = _dbcontext.JewelTypeMst.Where(x => x.product_id == product_id && !x.Deleted).FirstOrDefault();
                        if(checkXuLyDH != null)
                        {
                            checkXuLyDH.Deleted = true;
                            checkProduct.CretorEdit = checkAccount.username + " là người đã sửa bản ghi này vào lúc " + DateTimeOffset.UtcNow;
                            _dbcontext.JewelTypeMst.Update(checkXuLyDH);
                        }
                    }
                    if(status != Status.Delete && status != Status.Done)
                    {
                        checkProduct.trangthai = status.ToString();
                        checkProduct.CretorEdit = checkAccount.username + " là người đã sửa bản ghi này vào lúc " + DateTimeOffset.UtcNow;
                        _dbcontext.JewelTypeMst.Update(checkProduct);
                    }
                    if(status == Status.Done)
                    {
                        var listItem = new List<XulydonhangDTO>();
                        var mapDTO = _mapper.Map<XulydonhangDTO>(checkProduct);
                        mapDTO.account_id = checkProduct.Account.id;
                        listItem.Add(mapDTO);
                        await AddOrder(account_name, listItem);
                        checkProduct.Deleted = true;
                        checkProduct.trangthai = status.ToString();
                        checkProduct.CretorEdit = checkAccount.username + " là người đã sửa bản ghi này vào lúc " + DateTimeOffset.UtcNow;
                        _dbcontext.JewelTypeMst.Update(checkProduct);

                    }

                    if (await _dbcontext.SaveChangesAsync() > 0)
                    {
                        //Status status1;
                        var mapDTO = _mapper.Map<XulydonhangDTO>(checkProduct);
                        mapDTO.account_id = checkProduct.Account.id;
                        //if (Enum.TryParse(checkProduct.trangthai, true, out status1))
                        //{
                        //    mapDTO.trangthai = status1;
                        //}

                        // Kiểm tra giá trị enum chuyền lên từ client là hợp lệ và tồn tại trong enum "Status" không
                        if (Enum.TryParse(typeof(Status), checkProduct.trangthai, out object statusEnum) && Enum.IsDefined(typeof(Status), statusEnum))
                        {

                            mapDTO.trangthai = status;

                        }

                        return await Task.FromResult(PayLoad<XulydonhangDTO>.Successfully(mapDTO));
                    }

                }
                
                return await Task.FromResult(PayLoad<XulydonhangDTO>.CreatedFail("Faild Add"));
            }
            catch(Exception ex)
            {
                return await Task.FromResult(PayLoad<XulydonhangDTO>.CreatedFail(ex.Message));
            }
        }
    }
}