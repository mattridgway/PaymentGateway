using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.Sdk.Net
{
    internal class PaymentGatewayProxy : IPaymentGatewayProxy
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private readonly HttpClient _client;
        private readonly Uri _gateway;
        private readonly Uri _authUri;
        private readonly string _clientId;
        private readonly string _clientSecret;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Here the code should be specific about which configuration value is missing")]
        public PaymentGatewayProxy(HttpClient httpClient, IOptions<PaymentGatewayOptions> config)
        {
            _client = httpClient;
            _gateway = config.Value.PaymentGatewayUri;

            if (config.Value.AuthenticationUri == null)
                throw new ArgumentNullException(nameof(config.Value.AuthenticationUri));
            _authUri = config.Value.AuthenticationUri;

            if (string.IsNullOrWhiteSpace(config.Value.ClientId))
                throw new ArgumentNullException(nameof(config.Value.ClientId));
            _clientId = config.Value.ClientId;

            if (string.IsNullOrWhiteSpace(config.Value.ClientSecret))
                throw new ArgumentNullException(nameof(config.Value.ClientSecret));
            _clientSecret = config.Value.ClientSecret;
        }

        public async Task<string> AuthenticateAsync()
        {           
            var disco = await _client.GetDiscoveryDocumentAsync(_authUri.OriginalString);
            using var request = new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _clientId,
                ClientSecret = _clientSecret,
                Scope = "paymentgateway.process paymentgateway.retrieve"
            };
            var response = await _client.RequestClientCredentialsTokenAsync(request);

            _client.SetBearerToken(response.AccessToken);

            return response.AccessToken;
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
        {
            using var content = new StringContent(
                JsonSerializer.Serialize(
                    request,
                    _jsonOptions),
                Encoding.UTF8,
                "application/json");
            var response = await _client.PostAsync(new Uri($"{_gateway.AbsoluteUri}api/v1.0/payment"), content);
            return new PaymentResponse { StatusCode = (int)response.StatusCode };
        }

        public async Task<PaymentDetails> RetrievePaymentDetailsAsync(Guid paymentId)
        {
            var response = await _client.GetAsync(new Uri($"{_gateway.AbsoluteUri}api/v1.0/payments/{paymentId}"));
            return JsonSerializer.Deserialize<PaymentDetails>(
                await response.Content.ReadAsStringAsync(),
                _jsonOptions);
        }
    }
}
