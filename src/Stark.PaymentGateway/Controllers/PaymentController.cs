using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stark.PaymentGateway.Application.Payments.Commands;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult> ProcessPayment(PaymentRequest request)
        {
            var result = await _mediator.Send(
                CreateCommand(request, "merchant")); //TODO: Get the merchantid as a claim from the token when authentication is added

            return result switch
            {
                "PaymentRejected" => BadRequest(),
                "BadPaymentRequest" => BadRequest(),
                _ => Ok(),
            };
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
