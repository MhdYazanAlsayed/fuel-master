using FuelMaster.HeadOffice.Core.Configurations;

namespace FuelMaster.HeadOffice.Extensions.Dependencies.Configurations
{
    public static class TenantConfigurationExtension
    {
        public static IServiceCollection AddTenantConfiguration (this IServiceCollection services , IConfiguration configuration)
        {
            var multiTenantSettings = configuration
                .GetSection("MultiTenantSettings")
                .Get<TenantConfiguration>();

            if (multiTenantSettings is null)
                throw new Exception();

            services.AddSingleton(multiTenantSettings);

            return services;
        }
    }
}
