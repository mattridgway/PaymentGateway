using Microsoft.Extensions.DependencyInjection;
using System;

namespace Stark.PaymentGateway.Sdk.Net
{
    public static class IServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddPaymentGatewayProxy(
            this IServiceCollection services, 
            Uri paymentGatewayUri, 
            Uri authenticationServiceUri, 
            string authenticationClientId, 
            string authenticationClientSecret)
        {
            services.Configure<PaymentGatewayOptions>(options =>
            {
                options.PaymentGatewayUri = paymentGatewayUri;
                options.AuthenticationUri = authenticationServiceUri;
                options.ClientId = authenticationClientId;
                options.ClientSecret = authenticationClientSecret;
            });
            services.AddHttpClient<IPaymentGatewayProxy, PaymentGatewayProxy>();

            return services;
        }
    }
}
