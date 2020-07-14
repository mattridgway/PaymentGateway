namespace Stark.PaymentGateway.Domain.Payments.Entities
{
    public enum PaymentStatus
    {
        PaymentRaised = 0,
        PaymentSucceeded = 1,
        PaymentRejected = 2,
        BadPaymentRequest = 3
    }
}
