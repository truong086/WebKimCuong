using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Applications.Base.Models;
using Payment.Applications.Dtos;
using Payment.Applications.Features.Merchant.Commands;
using System.Net;

namespace thuongmaidientus1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantsController : ControllerBase
    {
        private readonly IMediator mediator;

        public MerchantsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        [ProducesResponseType(typeof(BaseResultWithData<List<MerchantDtos>>), 200)]
        [ProducesResponseType(400)]
        //public IActionResult Get(string criteria) => Ok();
        public IActionResult Get(string criteria)
        {
            var response = new BaseResultWithData<List<MerchantDtos>>();
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResultWithData<BasePagingData<MerchantDtos>>), 200)]
        [Route(nameof(GetPaging))]
        public IActionResult GetPaging([FromQuery] BasePagingQuery basePaging)
        {
            var response = new BaseResultWithData<BasePagingData<MerchantDtos>>();
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResultWithData<List<MerchantDtos>>), 200)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("{id}")]
        public IActionResult GetOne([FromRoute] string id)
        {
            var response = new BaseResultWithData<MerchantDtos>();
            return Ok(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResultWithData<List<MerchantDtos>>), 200)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create([FromBody] CreateMerchant createMerchant)
        {
            var response = new BaseResultWithData<MerchantDtos>();
            response = await mediator.Send(createMerchant);
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(BaseResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("{id}")]
        public IActionResult Update(string id, [FromBody] UpdateMerchant updateMerchant)
        {
            var response = new BaseResult();
            return Ok(response);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(BaseResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("{id}")]
        public IActionResult Delete(string id)
        {
            var response = new BaseResult();
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(BaseResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("{id}/active")]
        public IActionResult SetActive(string id, [FromBody] SetActiveMerchant setActiveMerchant)
        {
            var response = new BaseResult();
            return Ok(response);
        }
    }
}
