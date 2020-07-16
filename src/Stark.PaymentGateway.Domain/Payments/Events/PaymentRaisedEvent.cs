using MediatR;
using Newtonsoft.Json;
using Stark.ES;
using System;

namespace Stark.PaymentGateway.Domain.Payments.Events
{
    public class PaymentRaisedEvent : AggregateEvent, INotification
    {
        public string MerchantId { get; }
        public string CardNumber { get; }
        public int? ExpMonth { get; }
        public int? ExpYear { get; }
        public decimal? Amount { get; }
        public string Currency { get; }

        [JsonConstructor]
        internal PaymentRaisedEvent(Guid aggregateId, string merchantID, string cardNumber, int? expMonth, int? expYear, decimal? amount, string currency) : base(aggregateId)
        {
            MerchantId = merchantID;
            CardNumber = cardNumber;
            ExpMonth = expMonth;
            ExpYear = expYear;
            Amount = amount;
            Currency = currency;
        }
    }
}
