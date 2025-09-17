using FuelMaster.HeadOffice.Extensions.Dependencies.Configurations;
using System.Text.Json.Serialization;

namespace FuelMaster.HeadOffice.Extensions.Dependencies
{
    public static class DefaultDependeciesExtension
    {
        public static IServiceCollection RegisterDependencies (this IServiceCollection services ,
            IConfiguration configuration)
        {
            services.AddControllers()
                    .AddJsonOptions(configure =>
                    {
                        configure.JsonSerializerOptions.ReferenceHandler =
                        ReferenceHandler.IgnoreCycles;
                    });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSignalR();

            services
                .AddFuelMasterDbContext(configuration)
                .AddTenantConfiguration(configuration)
                .AddAuthenticationConfiguration(configuration)
                .AddPaginationConfiguration(configuration)
                .AddFuelMasterCors()
                .AddFuelMasterIdentity()
                .AddFuelMasterLocalization()
                .AddFuelMasterServices()
                .AddFuelMasterAuthentication(configuration)
                .AddHttpContextAccessor();

            return services;
        }
    }
}
