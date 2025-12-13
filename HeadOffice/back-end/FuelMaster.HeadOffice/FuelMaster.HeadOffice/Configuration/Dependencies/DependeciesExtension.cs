using FuelMaster.HeadOffice.Extensions.Dependencies.Configurations;
using FuelMaster.HeadOffice.Controllers.Cities.Validators;
using Serilog;
using Serilog.Events;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace FuelMaster.HeadOffice.Extensions.Dependencies
{
    public static class DependeciesExtension
    {
        public static IServiceCollection RegisterDependencies (this IServiceCollection services ,
            IConfiguration configuration)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .Filter.ByExcluding(e => e.Level == LogEventLevel.Warning && e.MessageTemplate.Text.Contains("Lucky Penny software MediatR"))
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
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();            


            services.AddControllers()
                    .AddJsonOptions(configure =>
                    {
                        configure.JsonSerializerOptions.ReferenceHandler =
                        ReferenceHandler.IgnoreCycles;
                    });

            // Register all validators in your assembly
            services.AddValidatorsFromAssemblyContaining<Program>();


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
                .AddServerSettingsConfiguration(configuration)
                .AddFuelMasterWebsiteConfiguration(configuration)
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
