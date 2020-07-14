using System;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Domain.Payments
{
    public interface IBankService
    {
        Task<BankResponse> RequestPaymentAsync(BankRequest request);
    }

    public class BankRequest
    {
        public Guid Id { get; }
        public string MerchantId { get; }
        public string CardNumber { get; }
        public int ExpMonth { get; }
        public int ExpYear { get; }
        public string CVV { get; }
        public decimal Amount { get; }
        public string Currency { get; }

        public BankRequest(Guid requestId, string merchantId, string cardNumber, int expMonth, int expYear, string cvv, decimal amount, string currency)
        {
            Id = requestId;
            MerchantId = merchantId;
            CardNumber = cardNumber;
            ExpMonth = expMonth;
            ExpYear = expYear;
            CVV = cvv;
            Amount = amount;
            Currency = currency;
        }
    }

    public class BankResponse
    {
        public Guid Id { get; }
        public bool IsSuccess { get; }
        public int StatusCode { get; }
        public string AuthCode { get; }
        public string ResponseReason { get; }
        public string BankName { get; }

        public BankResponse(Guid id, string bankName, bool isSuccess, int statusCode, string authCode, string responseReason)
        {
            Id = id;
            BankName = bankName;
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            AuthCode = authCode;
            ResponseReason = responseReason;
        }
    }
}
