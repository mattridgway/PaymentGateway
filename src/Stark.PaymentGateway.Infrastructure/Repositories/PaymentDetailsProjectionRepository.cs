using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stark.PaymentGateway.Application.Payments.Queries;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Infrastructure.Repositories
{
    internal class PaymentDetailsProjectionRepository : IReadPaymentDetailProjections
    {
        private readonly Container _container;

        public PaymentDetailsProjectionRepository(CosmosClient cosmos, IOptions<PaymentDetailProjectionCosmosOptions> config)
        {
            _container = cosmos.GetContainer(config.Value.DatabaseName, config.Value.ContainerName);
        }

        public async Task<PaymentDetailProjection> GetPaymentByIdAsync(Guid paymentId, string merchantId)
        {
            try
            {
                var result = await _container.ReadItemAsync<PaymentDetailProjectionEntity>(paymentId.ToString(), new PartitionKey(merchantId));
                return new PaymentDetailProjection(
                    new Guid(result.Resource.Id),
                    result.Resource.When,
                    result.Resource.Status,
                    result.Resource.IsSuccess,
                    result.Resource.MaskedCardNumber,
                    result.Resource.Amount,
                    result.Resource.Currency);
            }
            catch(CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ProjectionNotFoundException($"Projection with id {paymentId} not found in partition {merchantId}", ex);
            }            
        }

        private class PaymentDetailProjectionEntity
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("merchant")]
            public string Merchant { get; set; }

            [JsonProperty("when")]
            public DateTimeOffset When { get; set; }

            [JsonProperty("maskedCardNumber")]
            public string MaskedCardNumber { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("isSuccess")]
            public bool? IsSuccess { get; set; }

            [JsonProperty("amount")]
            public decimal Amount { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }
        }
    }


    [Serializable]
    public class ProjectionNotFoundException : Exception
    {
        public ProjectionNotFoundException() { }
        public ProjectionNotFoundException(string message) : base(message) { }
        public ProjectionNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ProjectionNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    internal class PaymentDetailProjectionCosmosOptions
    {
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public string PartitionKey { get; } = "/merchant";
    }

    internal class CosmosStartupTask : IHostedService
    {
        private readonly IServiceProvider _provider;

        public CosmosStartupTask(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _provider.CreateScope();
            var eventStoreConfig = scope.ServiceProvider.GetRequiredService<IOptions<PaymentDetailProjectionCosmosOptions>>();
            var client = scope.ServiceProvider.GetRequiredService<CosmosClient>();
            var dbresult = await client.CreateDatabaseIfNotExistsAsync(eventStoreConfig.Value.DatabaseName);
            await dbresult.Database.CreateContainerIfNotExistsAsync(eventStoreConfig.Value.ContainerName, eventStoreConfig.Value.PartitionKey);
        }
    }
}
