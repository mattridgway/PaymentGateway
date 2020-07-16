using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Infrastructure.Repositories.PaymentDetail
{
    internal class PaymentDetailProjectorStartupTask : IHostedService
    {
        private readonly IServiceProvider _provider;

        public PaymentDetailProjectorStartupTask(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _provider.CreateScope();
            var projectorOptions = scope.ServiceProvider.GetRequiredService<IOptions<PaymentDetailProjectionCosmosOptions>>();
            var client = scope.ServiceProvider.GetRequiredService<CosmosClient>();
            var dbresult = await client.CreateDatabaseIfNotExistsAsync(projectorOptions.Value.DatabaseName);
            await dbresult.Database.CreateContainerIfNotExistsAsync(projectorOptions.Value.ContainerName, projectorOptions.Value.PartitionKey);
        }
    }
}
