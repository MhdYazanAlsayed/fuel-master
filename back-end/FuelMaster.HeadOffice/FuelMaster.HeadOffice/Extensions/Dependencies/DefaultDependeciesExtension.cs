using FuelMaster.HeadOffice.Extensions.Dependencies.Configurations;
using Serilog;
using System.Text.Json.Serialization;

namespace FuelMaster.HeadOffice.Extensions.Dependencies
{
    public static class DefaultDependeciesExtension
    {
        public static IServiceCollection RegisterDependencies (this IServiceCollection services ,
            IConfiguration configuration)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(errorLogger => errorLogger
                    .Filter.ByIncludingOnly(e => e.Level >= Serilog.Events.LogEventLevel.Error)
                    .WriteTo.File(
                        path: "Logs/Errors/error-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} {Message:lj}{NewLine}{Exception}"))
                .WriteTo.Logger(infoLogger => infoLogger
                    .Filter.ByIncludingOnly(e => e.Level < Serilog.Events.LogEventLevel.Error)
                    .WriteTo.File(
                        path: "Logs/Information/info-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} {Message:lj}{NewLine}{Exception}"))
                .CreateLogger();            


            services.AddControllers()
                    .AddJsonOptions(configure =>
                    {
                        configure.JsonSerializerOptions.ReferenceHandler =
                        ReferenceHandler.IgnoreCycles;
                    });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSignalR();
            services.AddMemoryCache();

            services
                .AddFuelMasterDbContext(configuration)
                .AddTenantConfiguration(configuration)
                .AddAuthenticationConfiguration(configuration)
                .AddPaginationConfiguration(configuration)
                .AddCacheConfiguration(configuration)
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
