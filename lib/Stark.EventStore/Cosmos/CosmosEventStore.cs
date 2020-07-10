using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stark.Encryption;
using Stark.ES;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Stark.EventStore.Cosmos
{
    internal class CosmosEventStore : IStoreEvents
    {
        private readonly Container _container;
        private readonly IEncryptObjects _encryptor;
        private readonly ILogger<CosmosEventStore> _logger;

        public CosmosEventStore(CosmosClient client, IEncryptObjects encryptor, IOptions<CosmosEventStoreOptions> options, ILogger<CosmosEventStore> logger)
        {
            _container = client.GetContainer(options.Value.DatabaseName, options.Value.ContainerName);
            _encryptor = encryptor;
            _logger = logger;
        }

        public async Task SaveAggregateEventsAsync(string aggregateId, IEnumerable<AggregateEvent> aggregateEvents)
        {
            var batch = _container.CreateTransactionalBatch(new PartitionKey(aggregateId));
            foreach (var aggregateEvent in aggregateEvents)
            {
                var payload = await _encryptor.EncryptAsync(aggregateEvent);
                batch.CreateItem(
                    new CosmosEventStoreEntity(
                        aggregateEvent.SourceId.ToString(),
                        aggregateEvent.Version.ToString("D10", CultureInfo.InvariantCulture),
                        aggregateEvent.GetType().FullName,
                        payload));
            }
            var result = await batch.ExecuteAsync();
            _logger.LogTrace($"Events saved. Cosmos charge was: {result.RequestCharge}, with status code: {result.StatusCode}");

            // TODO: Deal with failures            
        }
    }
}
