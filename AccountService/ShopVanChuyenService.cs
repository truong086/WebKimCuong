using AutoMapper;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using thuongmaidientus1.Common;
using thuongmaidientus1.EmailConfig;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.AccountService
{
    public class ShopVanChuyenService : IShopVanChuyenService
    {
        private readonly DBContexts _dbcontext;
        private readonly IMapper _mapper;
        private readonly EmailSetting _emaiSetting;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        public ShopVanChuyenService(DBContexts dbcontext, IMapper mapper, IOptions<EmailSetting> emailSetting, IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _emaiSetting = emailSetting.Value;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }
        public async Task<PayLoad<List<ShopVanchuyenDTO>>> FindOneIdShop(int id)
        {
            try
            {
                var checkIdShop = _dbcontext.StoneMsts.Include(s => s.shop).Include(v => v.Vanchuyen).Where(x => x.shop.id == id).Select(x => new
                {
                    Id = x.id,
                    ShopId = x.shop.id,
                    VanChuyenId = x.Vanchuyen.id,
                    ShopName = x.shop.Name,
                    VanchuyenName = x.Vanchuyen.name

                }).ToList();
                if(checkIdShop == null)
                {
                    return await Task.FromResult(PayLoad<List<ShopVanchuyenDTO>>.CreatedFail("Inquiry chưa tôn tại"));
                }
                var listDTO = new List<ShopVanchuyenDTO>();
                for(var i = 0; i < checkIdShop.Count(); i++)
                {
                    var data = checkIdShop[i];
                    var MapDTO = new ShopVanchuyenDTO()
                    {
                        Id = data.Id,
                        ShopName = data.ShopName,
                        IdShop = data.ShopId,
                        VanchuyenName = data.VanchuyenName,
                        IdVanchuyen = data.VanChuyenId
                    };

                    listDTO.Add(MapDTO);
                }

                return await Task.FromResult(PayLoad<List<ShopVanchuyenDTO>>.Successfully(listDTO));
                
            }catch(Exception ex) {
                return await Task.FromResult(PayLoad<List<ShopVanchuyenDTO>>.CreatedFail(ex.Message));
            }
        }
    }
}
