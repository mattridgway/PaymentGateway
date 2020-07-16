using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stark.PaymentGateway.Application.Payments.Commands;
using Stark.PaymentGateway.Domain.Payments;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "ProcessPayment")]
    [Route("api/v{version:apiVersion}/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Consumes(MediaTypeNames.Application.Json)]
        [HttpPost(Name = "Process a payment")]
        public async Task<ActionResult> ProcessPayment(PaymentRequest request)
        {
            try
            {
                var merchant = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "client_Merchant").Value;
                var result = await _mediator.Send(CreateCommand(request, merchant)); 

                return result switch
                {
                    "PaymentRejected" => BadRequest(),
                    "BadPaymentRequest" => BadRequest(),
                    _ => Ok(),
                };
            }
            catch (PaymentAggregateException)
            {
                return BadRequest();
            }            
        }

        private static PaymentRequestCommand CreateCommand(PaymentRequest request, string merchantId)
        {
            return new PaymentRequestCommand(
                request.Id,
                merchantId,
                request.CardNumber,
                request.ExpMonth,
                request.ExpYear,
                request.CVV,
                request.Amount,
                request.Currency);
        }
    }
}
