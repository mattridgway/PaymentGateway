using Newtonsoft.Json;

namespace Stark.EventStore.Cosmos
{
    internal class CosmosEventStoreEntity
    {
        [JsonProperty("partitionKey")]
        public string AggregateID { get; set; }

        [JsonProperty("id")]
        public string Version { get; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("eventPayload")]
        public byte[] EncryptedPayload { get; set; }

        public CosmosEventStoreEntity(string aggregateID, string version, string eventType, byte[] encryptedPayload)
        {
            AggregateID = aggregateID;
            Version = version;
            EventType = eventType;
            EncryptedPayload = encryptedPayload;
        }
    }
}
