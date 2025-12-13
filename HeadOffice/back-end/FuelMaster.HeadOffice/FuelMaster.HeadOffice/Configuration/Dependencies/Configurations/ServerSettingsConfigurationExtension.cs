using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy.Configs;
using FuelMaster.HeadOffice.Infrastructure.Configurations;

namespace FuelMaster.HeadOffice.Extensions.Dependencies.Configurations
{
    public static class ServerSettingsConfigurationExtension
    {
        public static IServiceCollection AddServerSettingsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var serverSettings = configuration
                .GetSection("ServerSettings")
                .Get<ServerSettingsConfiguration>();

            if (serverSettings is null)
                throw new Exception("ServerSettings configuration not found in appsettings.json");

            services.AddSingleton<IServerSettingsConfiguration>(serverSettings);

            return services;
        }
    }
}

