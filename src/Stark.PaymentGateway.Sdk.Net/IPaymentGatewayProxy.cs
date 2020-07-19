using System;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Sdk.Net
{
    public interface IPaymentGatewayProxy
    {
        Task<string> AuthenticateAsync();
        Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
        Task<PaymentDetails> RetrievePaymentDetailsAsync(Guid paymentId);
    }
}
