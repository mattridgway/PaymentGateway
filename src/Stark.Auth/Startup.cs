using System.Collections.Generic;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Stark.Auth
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: Store in a database and use real key for signing.
            services.AddIdentityServer()
                .AddInMemoryApiScopes(
                    new List<ApiScope>
                    {
                        new ApiScope(name: "paymentgateway.process", displayName: "Process a new payment."),
                        new ApiScope(name: "paymentgateway.retrieve", displayName: "Retreieve details for existing payment.")
                    }
                )
                .AddInMemoryApiResources(
                    new List<ApiResource>
                    {
                        new ApiResource(name: "paymentgateway", displayName: "Stark Payment Gateway API.")
                        {
                            Scopes = { "paymentgateway.process", "paymentgateway.retrieve" }
                        }
                    })
                .AddInMemoryClients(
                    new List<Client>
                    {
                        new Client
                        {
                            ClientId = "sampleapplication",
                            ClientName = "Sample Application",
                            Description = "Sample application used for local development and testing the payment gateway",
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets = { new Secret("secret".Sha256()) },
                            AlwaysSendClientClaims = true,
                            AllowedScopes = { "paymentgateway.process", "paymentgateway.retrieve" },
                            Claims = { new ClientClaim("Merchant", "SampleApplication") },
                            RequireConsent = false,
                        }
                    })
                .AddDeveloperSigningCredential();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }
    }
}
