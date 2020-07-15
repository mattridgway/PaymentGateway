using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stark.Encryption;
using Stark.EventStore.Cosmos;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Stark.EventStore
{
    public static class IServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services, IConfiguration configuration, string applicationName)
        {
            services.AddEncryptor(configuration);

            services.AddHostedService<EventStoreStartupTask>();

            services.TryAddSingleton((services) => {
                return new CosmosClient(
                    configuration.GetConnectionString("CosmosEventStore"),
                    new CosmosClientOptions { ApplicationName = applicationName });
            });

            services.AddTransient<IStoreEvents, CosmosEventStore>();
            services.Configure<CosmosEventStoreOptions>(config =>
            {
                config.DatabaseName = configuration.GetValue<string>("EventStore:Cosmos:DatabaseName"); ;
                config.ContainerName = configuration.GetValue<string>("EventStore:Cosmos:ContainerName"); ;
            });            

            return services;
        }
    }
}
