using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stark.PaymentGateway.Application.Payments.Queries;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Infrastructure.Repositories.PaymentDetail
{
    internal class PaymentDetailProjectionRepository : IPaymentDetailProjectionRepository
    {
        private readonly Container _container;

        public PaymentDetailProjectionRepository(CosmosClient cosmos, IOptions<PaymentDetailProjectionCosmosOptions> config)
        {
            _container = cosmos.GetContainer(config.Value.DatabaseName, config.Value.ContainerName);
        }

        public async Task CreatePaymentProjectionAsync(PaymentDetailProjection projection)
        {
            await _container.CreateItemAsync(
                new PaymentDetailProjectionEntity
                {
                    Id = projection.Id.ToString(),
                    Merchant = projection.MerchantId,
                    When = projection.When,
                    Status = projection.Status,
                    MaskedCardNumber = projection.MaskedCardNumber,
                    Amount = projection.Amount,
                    Currency = projection.Currency,
                    IsSuccess = null,
                });
        }

        public async Task<PaymentDetailProjection> GetPaymentByIdAsync(Guid paymentId, string merchantId)
        {
            try
            {
                var result = await _container.ReadItemAsync<PaymentDetailProjectionEntity>(paymentId.ToString(), new PartitionKey(merchantId));
                return new PaymentDetailProjection(
                    new Guid(result.Resource.Id),
                    result.Resource.Merchant,
                    result.Resource.When,
                    result.Resource.Status,
                    result.Resource.IsSuccess,
                    result.Resource.MaskedCardNumber,
                    result.Resource.Amount,
                    result.Resource.Currency);
            }
            catch(CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new PaymentDetailNotFoundException($"Projection with id {paymentId} not found in partition {merchantId}", ex);
            }            
        }

        public async Task UpdatePaymentStatusProjection(Guid paymentId, string merchantId, string status, bool isSuccess)
        {
            var entity = await _container.ReadItemAsync<PaymentDetailProjectionEntity>(paymentId.ToString(), new PartitionKey(merchantId));
            entity.Resource.Status = status;
            entity.Resource.IsSuccess = isSuccess;

            await _container.ReplaceItemAsync(
                item: entity.Resource, 
                id: paymentId.ToString(), 
                partitionKey: new PartitionKey(merchantId));
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
            public decimal? Amount { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }
        }
    }
}
