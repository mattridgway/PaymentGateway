using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Stark.PaymentGateway.ComponentTests.ProcessPayment
{
    public class PaymentRequestTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly string _processPaymentUri;

        public PaymentRequestTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _processPaymentUri = "api/v1/payment";
        }

        [Fact(DisplayName = "GIVEN a valid payment request " +
                            "WHEN calling the payment api " +
                            "THEN return 200")]
        public async Task ValidPayment()
        {
            var paymentrequest = CreatePaymentRequest();
            var response = await _client.PostAsync(_processPaymentUri, paymentrequest);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private static HttpContent CreatePaymentRequest()
        {
            var payment = new PaymentRequest 
            { 
                Id = Guid.NewGuid(), 
                CardNumber = "4242424242424242",
                ExpMonth = 12,
                ExpYear = 99,
                CVV = 123,
                Amount = 9.99,
                Currency = "GBP"
            };

            return new StringContent(
                JsonSerializer.Serialize(payment, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                Encoding.UTF8,
                "application/json");
        }
    }
}
