using MediatR;
using Stark.PaymentGateway.Domain.Payments.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Application.Payments.Queries
{
    internal class PaymentDetailProjector :
        INotificationHandler<PaymentRaisedEvent>,
        INotificationHandler<PaymentRejectedEvent>,
        INotificationHandler<PaymentSucceededEvent>
    {
        private readonly IPaymentDetailProjectionRepository _repository;

        public PaymentDetailProjector(IPaymentDetailProjectionRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(PaymentSucceededEvent notification, CancellationToken cancellationToken)
        {
            await _repository.UpdatePaymentStatusProjection(notification.AggregateId, notification.MerchantId, notification.ResponseReason, true);
        }

        public async Task Handle(PaymentRejectedEvent notification, CancellationToken cancellationToken)
        {
            await _repository.UpdatePaymentStatusProjection(notification.AggregateId, notification.MerchantId, notification.ResponseReason, false);
        }

        public async Task Handle(PaymentRaisedEvent notification, CancellationToken cancellationToken)
        {
            await _repository.CreatePaymentProjectionAsync(
                new PaymentDetailProjection(
                    id: notification.AggregateId,
                    merchantId: notification.MerchantId,
                    when: DateTime.UtcNow,
                    status: "Raised",
                    isSuccess: null,
                    maskedCardNumber: Mask(notification.CardNumber),
                    amount: notification.Amount.Value,
                    currency: notification.Currency));
        }

        private string Mask(string input, int charToKeep = 4)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            if (input.Length < charToKeep)
                return input;

            return $"{new string('*', input.Length - charToKeep)}{input.Substring(input.Length - charToKeep)}";
        }
    }
}
