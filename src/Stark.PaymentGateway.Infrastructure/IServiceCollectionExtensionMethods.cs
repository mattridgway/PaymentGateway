using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stark.EventStore;
using Stark.PaymentGateway.Application.Payments.Queries;
using Stark.PaymentGateway.Domain;
using Stark.PaymentGateway.Domain.Payments;
using Stark.PaymentGateway.Infrastructure.ChangeFeed;
using Stark.PaymentGateway.Infrastructure.Repositories;
using Stark.PaymentGateway.Infrastructure.Repositories.PaymentDetail;
using Stark.PaymentGateway.Infrastructure.Services.Banks;

namespace Stark.PaymentGateway.Infrastructure
{
    public static class IServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddPaymentGatewayServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddSingleton((services) => {
                return new CosmosClient(
                    configuration.GetConnectionString("CosmosEventStore"),
                    new CosmosClientOptions { ApplicationName = "Stark.PaymentGateway" });
            });

            services.AddSingleton<IBankService, FakeBankService>();

            services.AddTransient(typeof(IAggregateRepository<>), typeof(EventSourcedAggregateRepository<>));
            services.AddEventStore(configuration, "Stark.PaymentGateway");

            services.AddHostedService<ChangeFeedSubscriberStartupTask>();
            services.AddHostedService<ChangeFeedSubscriber>();
            services.Configure<ChangeFeedSubscriberOptions>(config =>
            {
                config.DatabaseName = configuration.GetValue<string>("EventStore:Cosmos:DatabaseName");
                config.DataContainerName = configuration.GetValue<string>("EventStore:Cosmos:ContainerName");
                config.LeaseContainerName = configuration.GetValue<string>("EventStore:Cosmos:LeaseContainerName");
            });

            services.AddHostedService<PaymentDetailProjectorStartupTask>();
            services.AddTransient<IPaymentDetailProjectionRepository, PaymentDetailProjectionRepository>();
            services.Configure<PaymentDetailProjectionCosmosOptions>(config =>
            {
                config.DatabaseName = configuration.GetValue<string>("Projections:PaymentDetails:DatabaseName");
                config.ContainerName = configuration.GetValue<string>("Projections:PaymentDetails:ContainerName");
            });

            return services;
        }
    }
}
