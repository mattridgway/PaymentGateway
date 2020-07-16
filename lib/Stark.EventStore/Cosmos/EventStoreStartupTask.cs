using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stark.EventStore.Cosmos
{
    internal class EventStoreStartupTask : IHostedService
    {
        private readonly IServiceProvider _provider;

        public EventStoreStartupTask(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _provider.CreateScope();
            var eventStoreConfig = scope.ServiceProvider.GetRequiredService<IOptions<CosmosEventStoreOptions>>();
            var client = scope.ServiceProvider.GetRequiredService<CosmosClient>();
            var dbresult = await client.CreateDatabaseIfNotExistsAsync(eventStoreConfig.Value.DatabaseName);
            await dbresult.Database.CreateContainerIfNotExistsAsync(eventStoreConfig.Value.ContainerName, eventStoreConfig.Value.PartitionKey);
        }        
    }
}
