using MediatR;
using Newtonsoft.Json;
using Stark.ES;
using System;

namespace Stark.PaymentGateway.Domain.Payments.Events
{
    public class PaymentRejectedEvent : AggregateEvent, INotification
    {
        public string MerchantId { get; }
        public string BankName { get; }
        public int ResponseCode { get; }
        public string ResponseReason { get; }

        [JsonConstructor]
        internal PaymentRejectedEvent(Guid aggregateId, string merchantId, string bankName, int responseCode, string responseReason) : base(aggregateId)
        {
            MerchantId = merchantId;
            BankName = bankName;
            ResponseCode = responseCode;
            ResponseReason = responseReason;
        }
    }
}
