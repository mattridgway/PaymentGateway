using Stark.PaymentGateway.Application.Payments.Queries;
using System;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Infrastructure.Repositories
{
    internal class PaymentDetailsProjectionRepository : IReadPaymentDetailProjections
    {
        public Task<PaymentDetailProjection> GetPaymentByIdAsync(Guid paymentId, string merchantId)
        {
            throw new NotImplementedException();
        }
    }
}
