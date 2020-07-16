using Newtonsoft.Json;
using System;

namespace Stark.PaymentGateway.Infrastructure.Repositories.PaymentDetail
{
    internal partial class PaymentDetailProjectionRepository
    {
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
