using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stark.PaymentGateway.Application;
using Stark.PaymentGateway.Infrastructure;

namespace Stark.PaymentGateway
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(ApplicationLayer));

            services.AddHealthChecks();
            services.AddApiSecurity(_configuration);
            services.AddApiDocumentation();
            services.AddControllers();
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            });

            services.AddPaymentGatewayServices(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Stark Payment Gateway v1.0");
                config.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
