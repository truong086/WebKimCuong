using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thuongmaidientus1.AccountService;
using thuongmaidientus1.Common;
using thuongmaidientus1.ViewModel;

namespace thuongmaidientus1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhGiaController : ControllerBase
    {
        private readonly IDanhGiaService _danhgiaService;
        public DanhGiaController(IDanhGiaService danhGiaService)
        {
            _danhgiaService = danhGiaService;
        }

        [HttpGet]
        [Route(nameof(FindAll))]
        public async Task<PayLoad<object>> FindAll(string id, int page, int pageSize)
        {
            return await _danhgiaService.FindAll(id, page, pageSize);
        }

        [HttpPost]
        [Route(nameof(CreateDanhGia))]
        public async Task<PayLoad<DanhgiaDTO>> CreateDanhGia(DanhgiaDTO danhgia)
        {
            return await _danhgiaService.AddDanhgia(danhgia);
        }
    }
}
