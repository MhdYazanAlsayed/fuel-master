using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Mapper;
using Scrutor;
using System.Reflection;

namespace FuelMaster.HeadOffice.Extensions.Dependencies
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddFuelMasterServices (this IServiceCollection services)
        {
            var assemblies = LoadAssemblies();

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
             .UsingRegistrationStrategy(RegistrationStrategy.Skip)
             .AsImplementedInterfaces()
             .WithTransientLifetime());

            services.AddAutoMapper(typeof(FuelMasterMapper));

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
