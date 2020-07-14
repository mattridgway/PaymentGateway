using System;

namespace Stark.PaymentGateway.Domain.Payments
{

    [Serializable]
    public class PaymentAggregateException : Exception
    {
        public PaymentAggregateException() { }
        public PaymentAggregateException(string message) : base(message) { }
        public PaymentAggregateException(string message, Exception inner) : base(message, inner) { }
        protected PaymentAggregateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
