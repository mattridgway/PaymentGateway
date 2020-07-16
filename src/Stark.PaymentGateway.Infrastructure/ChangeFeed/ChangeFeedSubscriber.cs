using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stark.Encryption;
using Stark.EventStore.Cosmos;
using Stark.PaymentGateway.Domain.Payments.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Infrastructure.ChangeFeed
{
    internal class ChangeFeedSubscriber : IHostedService
    {
        private readonly IMediator _mediator;
        private readonly IEncryptObjects _encryptor;
        private readonly ILogger<ChangeFeedSubscriber> _logger;
        private readonly ChangeFeedProcessor _processor;

        public ChangeFeedSubscriber(
            CosmosClient client,
            IMediator mediator,
            IEncryptObjects encryptor,
            IOptions<ChangeFeedSubscriberOptions> config,
            ILogger<ChangeFeedSubscriber> logger)
        {
            _mediator = mediator;
            _encryptor = encryptor;
            _logger = logger;

            var container = client.GetContainer(config.Value.DatabaseName, config.Value.DataContainerName);

            _processor = container.GetChangeFeedProcessorBuilder<CosmosEventStoreEntity>("EventStoreProcessor", OnChange)
                .WithInstanceName($"PaymentGateway_{Guid.NewGuid()}")
                .WithLeaseContainer(client.GetContainer(config.Value.DatabaseName, config.Value.LeaseContainerName))
                .WithStartTime(DateTime.MinValue.ToUniversalTime())
                .Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _processor.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _processor.StopAsync();
        }

        private async Task OnChange(IReadOnlyCollection<CosmosEventStoreEntity> changes, CancellationToken token)
        {
            foreach (var change in changes)
            {
                try
                {
                    // TODO: Can I modify this so I don't need to update every time there is a new event type
                    switch (change.EventType)
                    {
                        case "Stark.PaymentGateway.Domain.Payments.Events.PaymentRaisedEvent":
                            var raisedEvent = await _encryptor.DecryptAsync<PaymentRaisedEvent>(change.EncryptedPayload);
                            await _mediator.Publish(raisedEvent);
                            break;
                        case "Stark.PaymentGateway.Domain.Payments.Events.PaymentRejectedEvent":
                            var rejectedEvent = await _encryptor.DecryptAsync<PaymentRejectedEvent>(change.EncryptedPayload);
                            await _mediator.Publish(rejectedEvent);
                            break;
                        case "Stark.PaymentGateway.Domain.Payments.Events.PaymentSucceededEvent":
                            var succeededEvent = await _encryptor.DecryptAsync<PaymentSucceededEvent>(change.EncryptedPayload);
                            await _mediator.Publish(succeededEvent);
                            break;
                        default:
                            _logger.LogError("ChangeFeedProcessor had an unknown event type to handle");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing change feed");
                    throw;
                }
            }
        }
    }
}
