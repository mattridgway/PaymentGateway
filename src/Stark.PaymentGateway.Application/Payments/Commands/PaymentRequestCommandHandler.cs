using MediatR;
using Microsoft.Extensions.Logging;
using Stark.PaymentGateway.Domain;
using Stark.PaymentGateway.Domain.Payments;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Application.Payments.Commands
{
    public class PaymentRequestCommandHandler : IRequestHandler<PaymentRequestCommand, string>
    {
        private readonly IBankService _bank;
        private readonly IAggregateRepository<PaymentAggregate> _repository;
        private readonly ILogger<PaymentRequestCommandHandler> _logger;

        public PaymentRequestCommandHandler(IAggregateRepository<PaymentAggregate> repository, IBankService bank, ILogger<PaymentRequestCommandHandler> logger)
        {
            _bank = bank;
            _repository = repository;
            _logger = logger;
        }

        public async Task<string> Handle(PaymentRequestCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            _logger.LogTrace($"Handling {nameof(PaymentRequestCommand)} with id {command.Id}.");

            var payment = new PaymentAggregate(command.Id, command.MerchantId, command.CardNumber, command.ExpMonth, command.ExpYear, command.Amount, command.Currency);
            await payment.RequestAsync(_bank, command.CVV);
            await _repository.SaveAggregateAsync(payment);

            return payment.Status.ToString();
        }
    }
}
