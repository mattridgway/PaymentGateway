using System;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Application.Payments.Queries
{
    public interface IPaymentDetailProjectionRepository
    {
        Task<PaymentDetailProjection> GetPaymentByIdAsync(Guid paymentId, string merchantId);
        Task CreatePaymentProjectionAsync(PaymentDetailProjection projection);
        Task UpdatePaymentStatusProjection(Guid paymentId, string merchantId, string status, bool isSuccess);
    }
}
