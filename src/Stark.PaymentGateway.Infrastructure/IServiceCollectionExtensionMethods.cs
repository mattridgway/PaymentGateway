using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stark.EventStore;
using Stark.PaymentGateway.Application.Payments.Queries;
using Stark.PaymentGateway.Domain;
using Stark.PaymentGateway.Domain.Payments;
using Stark.PaymentGateway.Infrastructure.Repositories;
using Stark.PaymentGateway.Infrastructure.Services.Banks;

namespace Stark.PaymentGateway.Infrastructure
{
    public static class IServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddPaymentGatewayServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEventStore(configuration, "Stark.PaymentGateway");

            services.AddSingleton<IBankService, FakeBankService>();
            services.AddTransient(typeof(IAggregateRepository<>), typeof(EventSourcedAggregateRepository<>));
            services.AddTransient<IReadPaymentDetailProjections, PaymentDetailsProjectionRepository>();

            return services;
        }
    }
}
