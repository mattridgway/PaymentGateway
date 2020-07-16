using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stark.PaymentGateway.Application.Payments.Queries;
using Stark.PaymentGateway.Infrastructure.Repositories.PaymentDetail;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "RetrieveDetails")]
    [Route("api/v{version:apiVersion}/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentDetailProjectionRepository _repository;

        public PaymentsController(IPaymentDetailProjectionRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieve payment details.
        /// </summary>
        /// <remarks>
        /// This will return the details of a previous payment request.
        /// See <see cref="PaymentController.ProcessPayment(PaymentRequest)"/> to process a new payment.
        /// </remarks>
        /// <param name="id">Id of the payment.</param>
        /// <returns>The payment details.</returns>     
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [HttpGet("{id}", Name = "Retrieve payment details")]
        public async Task<ActionResult<PaymentDetails>> RetrievePayment(Guid id)
        {
            try
            {
                var merchant = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "client_Merchant").Value;
                var payment = await _repository.GetPaymentByIdAsync(id, merchant);
                return Ok(payment);
            }
            catch (PaymentDetailNotFoundException)
            {
                return NotFound();
            }            
        }
    }
}
