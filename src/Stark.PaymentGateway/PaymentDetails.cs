using System;

namespace Stark.PaymentGateway
{
    public class PaymentDetails
    {
        public Guid Id { get; set; }
        public bool? IsSuccess { get; set; }
        public string MaskedCardNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
