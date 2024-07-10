using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thuongmaidientus1.AccountService;
using thuongmaidientus1.Common;
using thuongmaidientus1.Models;

namespace thuongmaidientus1.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DimQltyMstController : ControllerBase
    {
        private readonly IVanChuyenService _vanchuyen;
        public DimQltyMstController(IVanChuyenService vanchuyen)
        {
            _vanchuyen = vanchuyen;
        }

        [HttpGet]
        [Route(nameof(findAll))]
        public async Task<PayLoad<object>> findAll (string? name, int page = 1, int pageSize = 10)
        {
            return await _vanchuyen.FindAll(name, page, pageSize);
        }

        [HttpGet]
        [Route(nameof(findOneIdOrName))]
        public async Task<PayLoad<object>> findOneIdOrName([FromQuery]IList<string> id)
        {
            return await _vanchuyen.FindOneIdOrName(id);
        }

        [HttpPost]
        [Route(nameof(AddVanChuyen))]
        public async Task<PayLoad<DimQltyMst>> AddVanChuyen(DimQltyMst vanchuyen)
        {
            return await _vanchuyen.AddVanChuyen(vanchuyen);
        }

        [HttpPut]
        [Route(nameof(EditVanChuyen))]
        public async Task<PayLoad<DimQltyMst>> EditVanChuyen(int id, DimQltyMst vanchuyen)
        {
            return await _vanchuyen.EditVanChuyen(id, vanchuyen);
        }

        [HttpDelete]
        [Route(nameof(AddVanChuyen))]
        public async Task<PayLoad<string>> DeleteVanChuyen([FromQuery]IList<string> id)
        {
            return await _vanchuyen.DeleteVanChuyen(id);
        }
    }
}
