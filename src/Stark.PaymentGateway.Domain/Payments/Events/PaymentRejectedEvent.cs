using Stark.ES;
using System;

namespace Stark.PaymentGateway.Domain.Payments.Events
{
    public class PaymentRejectedEvent : AggregateEvent
    {
        public string BankName { get; }
        public int ResponseCode { get; }
        public string ResponseReason { get; }

        internal PaymentRejectedEvent(Guid aggregateId,  string bankName, int responseCode, string responseReason) : base(aggregateId)
        {
            BankName = bankName;
            ResponseCode = responseCode;
            ResponseReason = responseReason;
        }
    }
}
