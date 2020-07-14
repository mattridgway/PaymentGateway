using Stark.ES;
using System;

namespace Stark.PaymentGateway.Domain.Payments.Events
{
    public class PaymentSucceededEvent : AggregateEvent
    {
        public string BankName { get; }
        public int ResponseCode { get; }
        public string ResponseReason { get; }
        public string AuthCode { get; }

        internal PaymentSucceededEvent(Guid aggregateId, string bankName, int responseCode, string responseReason, string authCode) : base(aggregateId)
        {
            BankName = bankName;
            ResponseCode = responseCode;
            ResponseReason = responseReason;
            AuthCode = authCode;
        }
    }
}
