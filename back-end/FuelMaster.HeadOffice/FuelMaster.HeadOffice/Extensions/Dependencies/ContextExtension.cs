using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Extensions.Dependencies
{
    public static class ContextExtension
    {
        public static IServiceCollection AddFuelMasterDbContext (this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<FuelMasterDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            return services;
        }
    }
}
