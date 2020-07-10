namespace Stark.EventStore.Cosmos
{
    internal class CosmosEventStoreOptions
    {
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public string PartitionKey { get; } = "/partitionKey";
    }
}
