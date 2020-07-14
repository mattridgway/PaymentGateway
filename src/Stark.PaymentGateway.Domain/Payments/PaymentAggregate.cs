using Stark.ES;
using Stark.PaymentGateway.Domain.Payments.Entities;
using Stark.PaymentGateway.Domain.Payments.Events;
using System;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Domain.Payments
{
    public class PaymentAggregate : EventSourcedAggregate
    {
        private Guid _id;

        public override Guid Id => _id;
        public string MerchantId { get; private set; }
        public CardDetails Card { get; private set; }
        public Amount Amount { get; private set; }
        public PaymentStatus Status { get; private set; }

        public PaymentAggregate(Guid? requestId, string merchantId, string cardNumber, int? expMonth, int? expYear, decimal? amount, string currency)
        {
            if (!requestId.HasValue)
                throw new PaymentAggregateException($"{nameof(PaymentAggregate)} ID must have a value");

            RaiseEvent(new PaymentRaisedEvent(requestId.Value, merchantId, cardNumber, expMonth, expYear, amount, currency));
        }

        public async Task RequestAsync(IBankService service, string cvv)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));

            var result = await service.RequestPaymentAsync(
                new BankRequest(
                    Id,
                    MerchantId,
                    Card.CardNumber,
                    Card.ExpMonth,
                    Card.ExpYear,
                    cvv,
                    Amount.Value,
                    Amount.Currency));

            if (result.IsSuccess)
                RaiseEvent(new PaymentSucceededEvent(Id, result.BankName, result.StatusCode, result.ResponseReason, result.AuthCode));
            else
                RaiseEvent(new PaymentRejectedEvent(Id, result.BankName, result.StatusCode, result.ResponseReason));
        }

        public void Apply(PaymentRaisedEvent aggregateEvent)
        {
            if (aggregateEvent == null)
                throw new ArgumentNullException(nameof(aggregateEvent));

            _id = aggregateEvent.SourceId;
            MerchantId = aggregateEvent.MerchantId;
            Status = PaymentStatus.PaymentRaised;
            Card = new CardDetails(aggregateEvent.CardNumber, aggregateEvent.ExpMonth, aggregateEvent.ExpYear);
            Amount = new Amount(aggregateEvent.Amount, aggregateEvent.Currency);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "This parameter is ignored as it is not currently used.")]
        public void Apply(PaymentSucceededEvent _)
        {
            Status = PaymentStatus.PaymentSucceeded;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "This parameter is ignored as it is not currently used.")]
        public void Apply(PaymentRejectedEvent _)
        {
            Status = PaymentStatus.PaymentRejected;
        }
    }
}
