using System;

namespace Stark.PaymentGateway.Infrastructure.Repositories.PaymentDetail
{
    [Serializable]
    public class PaymentDetailNotFoundException : Exception
    {
        public PaymentDetailNotFoundException() { }
        public PaymentDetailNotFoundException(string message) : base(message) { }
        public PaymentDetailNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected PaymentDetailNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
