using FuelMaster.HeadOffice.Core.Configurations;
using Microsoft.Extensions.Configuration;

namespace FuelMaster.HeadOffice.Extensions.Dependencies.Configurations
{
    public static class PaginationConfigurationExtension
    {
        public static IServiceCollection AddPaginationConfiguration (this IServiceCollection services , IConfiguration configuration)
        {
            var paginationSettings = configuration
                .GetSection("PaginationSetting")
                .Get<PaginationConfiguration>();

            if (paginationSettings is null)
                throw new Exception();

            services.AddSingleton(paginationSettings);

            return services;
        }
    }
}
