using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Infrastructure.ChangeFeed
{
    internal class ChangeFeedSubscriberStartupTask : IHostedService
    {
        private readonly IServiceProvider _provider;

        public ChangeFeedSubscriberStartupTask(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _provider.CreateScope();
            var changeFeedOptions = scope.ServiceProvider.GetRequiredService<IOptions<ChangeFeedSubscriberOptions>>();
            var client = scope.ServiceProvider.GetRequiredService<CosmosClient>();
            var dbresult = await client.CreateDatabaseIfNotExistsAsync(changeFeedOptions.Value.DatabaseName);
            await dbresult.Database.CreateContainerIfNotExistsAsync(changeFeedOptions.Value.DataContainerName, changeFeedOptions.Value.DataPartitionKey);
            await dbresult.Database.CreateContainerIfNotExistsAsync(changeFeedOptions.Value.LeaseContainerName, changeFeedOptions.Value.LeasePartitionKey);
        }
    }
}
