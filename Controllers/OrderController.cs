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
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderservice;
        public OrderController(IOrderService orderService)
        {
                _orderservice = orderService;
        }

        [HttpGet]
        [Route(nameof(FindAll))]
        public async Task<PayLoad<object>> FindAll(string? name, int page = 1, int pageSize = 10)
        {
            return await _orderservice.FindAll(name, page, pageSize);
        }

        [HttpGet]
        [Route(nameof(FindAllDonHangXuLy))]
        public async Task<PayLoad<object>> FindAllDonHangXuLy(string? name, int page = 1, int pageSize = 20)
        {
            return await _orderservice.FindAllDonHangXuLy(name, page, pageSize);
        }
        [HttpGet]
        [Route(nameof(FindAllDonHangByMax))]
        public async Task<PayLoad<object>> FindAllDonHangByMax(string? name, string? account_name, int page = 1, int pageSize = 20)
        {
            return await _orderservice.FindAllDonHangByMax(name, account_name, page, pageSize);
        }
        [HttpGet]
        [Route(nameof(FindAllAccountDonHangByMax))]
        public async Task<PayLoad<object>> FindAllAccountDonHangByMax(string? name, string? account_name, int page = 1, int pageSize = 20)
        {
            return await _orderservice.FindAllAccountDonHangByMax(name, account_name, page, pageSize);
        }

        [HttpGet]
        [Route(nameof(FindAllShopDonHangByMax))]
        public async Task<PayLoad<object>> FindAllShopDonHangByMax(string? name, string? account_name, int page = 1, int pageSize = 20)
        {
            return await _orderservice.FindAllShopDonHangByMax(name, account_name, page, pageSize);
        }

        [HttpGet]
        [Route(nameof(FindAllShopProductMax))]
        public async Task<PayLoad<object>> FindAllShopProductMax(string? name, string? account_name, int page = 1, int pageSize = 20)
        {
            return await _orderservice.FindAllShopProductMax(name, account_name, page, pageSize);
        }

        [HttpPost]
        [Route(nameof(AddOrder))]
        public async Task<PayLoad<CertifyMst>> AddOrder(string? name, IList<XulydonhangDTO> donhang)
        {
            return await _orderservice.AddOrder(name, donhang);
        }

        [HttpDelete]
        [Route(nameof(DeleteOrder))]
        public async Task<PayLoad<string>> DeleteOrder(IList<string> id)
        {
            return await _orderservice.DeleteOrder(id);
        }

        [HttpPost]
        [Route(nameof(AddDonHangXuLy))]
        public async Task<PayLoad<XulydonhangDTO>> AddDonHangXuLy(IList<XulydonhangDTO> xulydonhangs)
        {
            return await _orderservice.AddDonHangXuLy(xulydonhangs);
        }

        [HttpPut]
        [Route(nameof(UpdateDonHangXuLy))]
        public async Task<PayLoad<XulydonhangDTO>> UpdateDonHangXuLy(int product_id, string? name, string? accounty_name, Status? status = Status.Pending)
        {
            return await _orderservice.UpdateDonHangXuLy(product_id, name, accounty_name, status);
        }
    }
}
