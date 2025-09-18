using FuelMaster.HeadOffice.Core.Configurations;
using Microsoft.Extensions.Configuration;

namespace FuelMaster.HeadOffice.Extensions.Dependencies.Configurations
{
    public static class CacheConfigurationExtension
    {
        public static IServiceCollection AddCacheConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheSettings = configuration
                .GetSection("CacheSettings")
                .Get<CacheConfiguration>();

            if (cacheSettings is null)
                throw new Exception("CacheSettings configuration not found in appsettings.json");

            services.Configure<CacheConfiguration>(configuration.GetSection("CacheSettings"));
            services.AddSingleton(cacheSettings);

            return services;
        }
    }
}
