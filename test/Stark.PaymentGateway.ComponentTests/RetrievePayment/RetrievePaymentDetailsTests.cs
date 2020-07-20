using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stark.PaymentGateway.Infrastructure.Repositories.PaymentDetail;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using static Stark.PaymentGateway.Infrastructure.Repositories.PaymentDetail.PaymentDetailProjectionRepository;

namespace Stark.PaymentGateway.ComponentTests.RetrievePayment
{
    public class RetrievePaymentDetailsTests : IClassFixture<AuthBypassWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly string _retrievePaymentUri;
        private readonly PaymentDetailProjectionEntity _paymentDetails;

        public RetrievePaymentDetailsTests(AuthBypassWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _retrievePaymentUri = "/api/v1.0/payments/";

            //TODO: Move test data setup to class fixture
            var cosmos = factory.Services.GetRequiredService<CosmosClient>();
            var config = factory.Services.GetRequiredService<IOptions<PaymentDetailProjectionCosmosOptions>>();
            var container = cosmos.GetContainer(config.Value.DatabaseName, config.Value.ContainerName);
            var paymentId = Guid.NewGuid();
            var merchant = "Automated Component Tests";
            _paymentDetails = new PaymentDetailProjectionEntity { Id = paymentId.ToString(), Merchant = merchant, Amount = 1.23M, Currency = "USD", MaskedCardNumber = "************4242", IsSuccess = true };
            container.CreateItemAsync(_paymentDetails, new PartitionKey(merchant)).GetAwaiter().GetResult();
        }

        [Fact(DisplayName = "GIVEN a valid payment id " +
                            "WHEN calling the payments api " +
                            "THEN return 200 with the payment details")]
        public async Task PaymentExists()
        {
            var response = await _client.GetAsync(_retrievePaymentUri + _paymentDetails.Id);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var details = JsonSerializer.Deserialize<PaymentDetails>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            details.Id.Should().Be(_paymentDetails.Id);
            details.Amount.Should().Be(_paymentDetails.Amount);
            details.Currency.Should().Be(_paymentDetails.Currency);
            details.IsSuccess.Should().Be(_paymentDetails.IsSuccess);
            details.MaskedCardNumber.Should().Be(_paymentDetails.MaskedCardNumber);
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
