using FuelMaster.HeadOffice.Infrastructure.Configurations;

namespace FuelMaster.HeadOffice.Extensions.Dependencies.Configurations
{
    public static class FuelMasterWebsiteConfigurationExtension
    {
        public static IServiceCollection AddFuelMasterWebsiteConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var fuelMasterWebsiteSettings = configuration
                .GetSection("FuelMasterWebsite")
                .Get<FuelMasterWebsiteConfiguration>();

            if (fuelMasterWebsiteSettings is null)
                throw new Exception("FuelMasterWebsite configuration not found in appsettings.json");

            services.Configure<FuelMasterWebsiteConfiguration>(configuration.GetSection("FuelMasterWebsite"));
            services.AddSingleton(fuelMasterWebsiteSettings);

            return services;
        }
    }
}

