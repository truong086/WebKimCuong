using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thuongmaidientus1.AccountService;
using thuongmaidientus1.Common;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class InquiryController : ControllerBase
    {
        private readonly IShopService _shop;
        public InquiryController(IShopService shop)
        {
            _shop = shop;
        }

        [HttpGet]
        [Route(nameof(findAll))]
        public async Task<PayLoad<object>> findAll(string? name, int page = 1, int pageSize = 10)
        {
            return await _shop.FindAll(name, page, pageSize);
        }

        [HttpPost]
        [Route(nameof(AddShop))]
        public async Task<PayLoad<ShopDTO>> AddShop(ShopDTO shopDTO)
        {
            return await _shop.AddShop(shopDTO);
        }

        [HttpPut]
        [Route(nameof(EditShop))]
        public async Task<PayLoad<ShopDTO>> EditShop(int id, ShopDTO shopDTO, string? name)
        {
            return await _shop.EditShop(id, shopDTO, name);
        }

        [HttpDelete]
        [Route(nameof(DeleteShop))]
        public async Task<PayLoad<string>> DeleteShop([FromHeader]IList<string> id)
        {
            return await _shop.DeleteShop(id);
        }

        [HttpGet]
        [Route(nameof(FindOneIdOrName))]
        public async Task<PayLoad<object>> FindOneIdOrName([FromHeader]IList<string> name)
        {
            return await _shop.FindOneIdOrName(name);
        }
    }
}
