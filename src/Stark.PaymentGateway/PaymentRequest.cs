using System;

namespace Stark.PaymentGateway
{
    public class PaymentRequest
    {
        public Guid Id { get; set; }
        public string CardNumber { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public int CVV { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
    }
}
