using Microsoft.Extensions.DependencyInjection;
using Stark.PaymentGateway.Sdk.Net;
using System;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.SampleApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddPaymentGatewayProxy(
                    paymentGatewayUri: new Uri("https://localhost:5001"),
                    authenticationServiceUri: new Uri("https://localhost:5101"),
                    authenticationClientId: "sampleapplication",
                    authenticationClientSecret: "secret"
                )
                .BuildServiceProvider();
            var proxy = serviceProvider.GetRequiredService<IPaymentGatewayProxy>();

            Console.WriteLine("Authenticating sample application");
            var token = await proxy.AuthenticateAsync();
            Console.WriteLine($"Token is {token}");

            Console.WriteLine("Making a new Payment Request");
            var request = new PaymentRequest
            {
                Id = Guid.NewGuid(),
                CardNumber = "4242424242424242",
                ExpMonth = 1,
                ExpYear = 22,
                CVV = "123",
                Amount = 9.99M,
                Currency = "GBP"
            };
            var response = await proxy.ProcessPaymentAsync(request);
            Console.WriteLine($"Response was: {response.StatusCode}");

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            Console.WriteLine("Requesting Payment Details");
            var details = await proxy.RetrievePaymentDetailsAsync(request.Id.Value);
            Console.WriteLine($"Payment for {details.Amount}{details.Currency} " +
                                $"with id {details.Id} " +
                                $"was found and the details were returned.");

            Console.Write("Press any key to close");
            Console.ReadKey();
        }
    }
}
