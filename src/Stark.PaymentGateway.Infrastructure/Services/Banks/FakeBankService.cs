using Stark.PaymentGateway.Domain.Payments;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Infrastructure.Services.Banks
{
    internal class FakeBankService : IBankService
    {
        private readonly Random _rng;

        public FakeBankService()
        {
            _rng = new Random();
        }

        public Task<BankResponse> RequestPaymentAsync(BankRequest request)
        {
            return Task.FromResult(
                new BankResponse(
                    request.Id,
                    "Stark Bank",
                    true,
                    200,
                    _rng.Next().ToString("D6", CultureInfo.InvariantCulture),
                    "Payment Accepted"));
        }
    }
}
