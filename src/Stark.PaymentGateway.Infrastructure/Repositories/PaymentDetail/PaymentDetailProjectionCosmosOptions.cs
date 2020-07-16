namespace Stark.PaymentGateway.Infrastructure.Repositories.PaymentDetail
{
    internal class PaymentDetailProjectionCosmosOptions
    {
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public string PartitionKey { get; } = "/merchant";
    }
}
