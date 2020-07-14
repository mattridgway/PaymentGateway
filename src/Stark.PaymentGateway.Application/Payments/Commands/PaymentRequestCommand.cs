using MediatR;
using Stark.CQRS;
using System;

namespace Stark.PaymentGateway.Application.Payments.Commands
{
    public class PaymentRequestCommand : Command, IRequest<string>
    {
        public string MerchantId { get; }
        public string CardNumber { get; }
        public int? ExpMonth { get; }
        public int? ExpYear { get; }
        public string CVV { get; }
        public decimal? Amount { get; }
        public string Currency { get; }

        public PaymentRequestCommand(Guid? paymentId, string merchantId, string cardNumber, int? expMonth, int? expYear, string cvv, decimal? amount, string currency) : base(paymentId)
        {
            MerchantId = merchantId;
            CardNumber = cardNumber;
            ExpMonth = expMonth;
            ExpYear = expYear;
            CVV = cvv;
            Amount = amount;
            Currency = currency;
        }
    }
}
