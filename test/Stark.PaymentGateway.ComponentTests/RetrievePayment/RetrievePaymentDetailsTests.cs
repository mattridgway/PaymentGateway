using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Stark.PaymentGateway.ComponentTests.RetrievePayment
{
    public class RetrievePaymentDetailsTests : IClassFixture<AuthBypassWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly string _retrievePaymentUri;

        public RetrievePaymentDetailsTests(AuthBypassWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _retrievePaymentUri = "/api/v1.0/payments/";

            //TODO: Setup to seed test data
        }

        [Fact(DisplayName = "GIVEN a valid payment id " +
                            "WHEN calling the payments api " +
                            "THEN return 200 with the payment details")]
        public async Task PaymentExists()
        {
            var response = await _client.GetAsync(_retrievePaymentUri + Guid.NewGuid().ToString());
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //TODO: Check contents matches what is expected
        }

        [Fact(DisplayName = "GIVEN id does not relate to an existing payment " +
                            "WHEN calling the payments api " +
                            "THEN return 404")]
        public async Task PaymentDoesNotExist()
        {
            var response = await _client.GetAsync(_retrievePaymentUri + Guid.NewGuid().ToString());
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            
        }
    }
}
