using System;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Application.Payments.Queries
{
    public interface IReadPaymentDetailProjections
    {
        Task<PaymentDetailProjection> GetPaymentByIdAsync(Guid paymentId, string merchantId);
    }
}
