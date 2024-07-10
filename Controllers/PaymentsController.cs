using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Payment.Applications.Base.Models;
using Payment.Applications.Dtos;
using Payment.Applications.Features.Merchant.Dtos;
using Payment.Applications.Features.Payment.Commands;
using Payment.Applications.Features.Payment.Dtos;
using Payment.Service.VnPay.Config;
using Payment.Service.VnPay.Response;
using Payment.Ultils.Extensions;
using System.Net;

namespace thuongmaidientus1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly VnpayConfig vnpayConfigOptions;

        public PaymentsController(IMediator mediator, IOptions<VnpayConfig> vnpayConfigOptions)
        {
            this.mediator = mediator;
            this.vnpayConfigOptions = vnpayConfigOptions.Value;
        }
        [HttpPost]
        [ProducesResponseType(typeof(BaseResultWithData<List<PaymentLinkDtos>>), 200)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create([FromBody] CreatePayment createPayment)
        {
            var response = new BaseResultWithData<PaymentLinkDtos>();
            response = await mediator.Send(createPayment);
            return Ok(response);
        }

        [HttpGet]
        [Route("vnpay-return")]
        public async Task<IActionResult> VnpayReturn([FromQuery] VnpayPayResponse response)
        {
            string returnUrl = string.Empty;
            var returnModel = new PaymentReturnDtos();
            var processResult = await mediator.Send(response.Adapt<ProcessVnpayPaymentReturn>());

            if (processResult.Success)
            {
                returnModel = processResult.Data.Item1 as PaymentReturnDtos;
                returnUrl = processResult.Data.Item2 as string;
            }
            if (returnUrl.EndsWith("/"))
                returnUrl = returnUrl.Remove(returnUrl.Length - 1, 1);

            return Redirect($"{returnUrl}?{returnModel.ToQueryString()}");
        }

    }
}
