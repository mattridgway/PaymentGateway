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
    public class PaymentRequestTests : IClassFixture<AuthBypassWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly string _processPaymentUri;

        public PaymentRequestTests(AuthBypassWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _processPaymentUri = "api/v1.0/payment";
        }

        [Fact(DisplayName = "GIVEN a valid payment request " +
                            "WHEN calling the payment api " +
                            "THEN return 200")]
        public async Task ValidPayment()
        {
            var paymentrequest = CreatePaymentRequest(Guid.NewGuid());
            var response = await _client.PostAsync(_processPaymentUri, paymentrequest);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact(DisplayName = "GIVEN an empty payment request " +
                            "WHEN calling the payment api " +
                            "THEN return 400")]
        public async Task EmptyPayment()
        {
            var paymentrequest = CreatePaymentRequest(null, null, null, null, null, null, null);
            var response = await _client.PostAsync(_processPaymentUri, paymentrequest);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private static HttpContent CreatePaymentRequest(Guid? id, string cardnumber = "4242424242424242", int? expMonth = 12, int? expYear = 99, string cvv = "123", decimal? amount = 9.99M, string currency = "GBP")
        {
            var payment = new PaymentRequest 
            { 
                Id = id, 
                CardNumber = cardnumber,
                ExpMonth = expMonth,
                ExpYear = expYear,
                CVV = cvv,
                Amount = amount,
                Currency = currency
            };

            return new StringContent(
                JsonSerializer.Serialize(payment, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                Encoding.UTF8,
                "application/json");
        }
    }
}
