namespace Stark.PaymentGateway.Infrastructure.ChangeFeed
{
    internal class ChangeFeedSubscriberOptions
    {
        public string DatabaseName { get; set; }
        public string DataContainerName { get; set; }
        public string DataPartitionKey { get; } = "/partitionKey";
        public string LeaseContainerName { get; set; }
        public string LeasePartitionKey { get; } = "/id";
    }
}
