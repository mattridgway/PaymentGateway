using Microsoft.AspNetCore.Mvc;
using Stark.PaymentGateway.Application.Payments.Queries;
using Stark.PaymentGateway.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IReadPaymentDetailProjections _repository;

        public PaymentsController(IReadPaymentDetailProjections repository)
        {
            _repository = repository;
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}", Name = "Retrieve payment details")]
        public async Task<ActionResult<PaymentDetails>> RetrievePayment(Guid id)
        {
            try
            {
                //TODO: Get the merchantid as a claim from the token when authentication is added
                var payment = await _repository.GetPaymentByIdAsync(id, "merchant");
                return Ok(payment);
            }
            catch (ProjectionNotFoundException)
            {
                return NotFound();
            }            
        }
    }
}
