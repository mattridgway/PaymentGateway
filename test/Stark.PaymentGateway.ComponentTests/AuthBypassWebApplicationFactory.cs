using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Stark.PaymentGateway.ComponentTests
{
    public class AuthBypassWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "BypassAuthHandler";
                    options.DefaultChallengeScheme = "BypassAuthHandler";
                }).AddScheme<AuthenticationSchemeOptions, BypassAuthHandler>("BypassAuthHandler", _ => { });
            });
        }

        private class BypassAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
        {
            public BypassAuthHandler(
                IOptionsMonitor<AuthenticationSchemeOptions> options,
                ILoggerFactory logger,
                UrlEncoder encoder,
                ISystemClock clock)
                : base(options, logger, encoder, clock) { }

            protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                var claims = new[] {
                    new Claim("client_Merchant", "Automated Component Tests", "http://www.w3.org/2001/XMLSchema#string"),
                    new Claim("scope", "paymentgateway.process", "http://www.w3.org/2001/XMLSchema#string"),
                    new Claim("scope", "paymentgateway.retrieve", "http://www.w3.org/2001/XMLSchema#string")
                };
                var identity = new ClaimsIdentity(claims);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, "Simple Authentication Scheme for Tests");

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
        }
    }
}
