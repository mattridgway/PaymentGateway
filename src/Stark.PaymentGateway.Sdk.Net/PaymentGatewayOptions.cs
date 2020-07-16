using System;

namespace Stark.PaymentGateway.Sdk.Net
{
    internal class PaymentGatewayOptions
    {
        public Uri PaymentGatewayUri { get; set; }
        public Uri AuthenticationUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
