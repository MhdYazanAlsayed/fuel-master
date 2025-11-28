using FuelMaster.HeadOffice.Application.Mappers;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Infrastructure.Services.Implementations.DB;
using FuelMaster.HeadOffice.Services;
using MediatR;
using Scrutor;
using System.Reflection;

namespace FuelMaster.HeadOffice.Extensions.Dependencies
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddFuelMasterServices (this IServiceCollection services)
        {
            var assemblies = LoadAssemblies();

            // Register MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

            services.Scan(scan => scan
            .FromAssemblies(assemblies)
             .AddClasses(classes => classes.AssignableTo<ISingletonDependency>())
             .UsingRegistrationStrategy(RegistrationStrategy.Skip)
             .AsImplementedInterfaces()
             .WithSingletonLifetime());

            services.Scan(scan => scan
             .FromAssemblies(assemblies)
             .AddClasses(classes => classes.AssignableTo<IScopedDependency>())
             .UsingRegistrationStrategy(RegistrationStrategy.Skip)
             .AsImplementedInterfaces()
             .WithScopedLifetime());

            services.Scan(scan => scan
             .FromAssemblies(assemblies)
             .AddClasses(classes => classes.AssignableTo<ITransientDependency>())
             .UsingRegistrationStrategy(RegistrationStrategy.Append)
             .AsImplementedInterfaces()
             .WithTransientLifetime());

            services.AddAutoMapper(typeof(FuelMasterMapper));

            // Register SeederDiscoveryService
            services.AddScoped<SeederDiscoveryService>();
            services.AddScoped<TanentService>();
            services.AddScoped<ITanentService>(sp => sp.GetRequiredService<TanentService>());

            return services;
        }

        private static Assembly[] LoadAssemblies ()
        {
            AppDomain.CurrentDomain.Load("FuelMaster.HeadOffice.ApplicationService");
            AppDomain.CurrentDomain.Load("FuelMaster.HeadOffice.Core");
            AppDomain.CurrentDomain.Load("FuelMaster.HeadOffice.Infrastructure");

            return AppDomain.CurrentDomain.GetAssemblies();
        }

    }
}
