using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Stark.PaymentGateway
{
    public static class IServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddApiSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration.GetValue<string>("Authentication:JWT:Authority");
                    options.RequireHttpsMetadata = true;
                    options.Audience = "paymentgateway";
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ProcessPayment", policy => policy.RequireClaim("scope", "paymentgateway.process"));
                options.AddPolicy("RetrieveDetails", policy => policy.RequireClaim("scope", "paymentgateway.retrieve"));
            });

            return services;
        }
    }
}
