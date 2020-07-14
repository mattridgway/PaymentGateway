using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stark.Encryption.AES;
using System.Text;

namespace Stark.Encryption
{
    public static class IServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddEncryptor(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEncryptObjects, AesEncryptor>();
            services.Configure<AesEncryptorOptions>(config =>
            {
                config.IV = Encoding.UTF8.GetBytes(configuration.GetValue<string>("Encryption:IV"));
                config.Key = Encoding.UTF8.GetBytes(configuration.GetValue<string>("Encryption:Key"));
            });

            return services;
        }
    }
}
