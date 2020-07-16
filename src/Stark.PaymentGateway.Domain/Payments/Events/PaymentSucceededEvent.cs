using MediatR;
using Newtonsoft.Json;
using Stark.ES;
using System;

namespace Stark.PaymentGateway.Domain.Payments.Events
{
    public class PaymentSucceededEvent : AggregateEvent, INotification
    {
        public string MerchantId { get; }
        public string BankName { get; }
        public int ResponseCode { get; }
        public string ResponseReason { get; }
        public string AuthCode { get; }

        [JsonConstructor]
        internal PaymentSucceededEvent(Guid aggregateId, string merchantId, string bankName, int responseCode, string responseReason, string authCode) : base(aggregateId)
        {
            MerchantId = merchantId;
            BankName = bankName;
            ResponseCode = responseCode;
            ResponseReason = responseReason;
            AuthCode = authCode;
        }
    }
}
