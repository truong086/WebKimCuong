using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thuongmaidientus1.AccountService;
using thuongmaidientus1.Common;
using thuongmaidientus1.Models;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class StoneMstController : ControllerBase
    {
        private readonly IShopVanChuyenService _shopvanchuyen;
        public StoneMstController(IShopVanChuyenService shopvanchuyen)
        {
            _shopvanchuyen = shopvanchuyen;
        }

        [HttpGet]
        [Route(nameof(FindOneIdShop))]
        public async Task<PayLoad<List<ShopVanchuyenDTO>>> FindOneIdShop(int id)
        {
            return await _shopvanchuyen.FindOneIdShop(id);
        }
    }
}
