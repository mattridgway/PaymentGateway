using System;

namespace Stark.PaymentGateway.Application.Payments.Queries
{
    public class PaymentDetailProjection
    {
        public Guid Id { get; }
        public string MerchantId { get; }
        public DateTimeOffset When { get; }
        public string Status { get; }
        public bool? IsSuccess { get; }
        public string MaskedCardNumber { get; }
        public decimal? Amount { get; }
        public string Currency { get; }

        public PaymentDetailProjection(Guid id, string merchantId, DateTimeOffset when, string status, bool? isSuccess, string maskedCardNumber, decimal? amount, string currency)
        {
            Id = id;
            MerchantId = merchantId;
            When = when;
            Status = status;
            IsSuccess = isSuccess;
            MaskedCardNumber = maskedCardNumber;
            Amount = amount;
            Currency = currency;
        }
    }
}
