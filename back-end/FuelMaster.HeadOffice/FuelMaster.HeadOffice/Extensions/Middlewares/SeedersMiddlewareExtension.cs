using FuelMaster.HeadOffice.ApplicationService.Database;

namespace FuelMaster.HeadOffice.Extensions.Middlewares
{
    public static class SeedersMiddlewareExtension
    {
        public static async Task<WebApplication> UseSeedersAsync (this WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var seederDiscoveryService = scope.ServiceProvider.GetService<SeederDiscoveryService>();
            
            if (seederDiscoveryService is null)
                throw new NullReferenceException("SeederDiscoveryService was not registered");

            await seederDiscoveryService.ExecuteAllSeedersAsync();

            return app;
        }
    }
}
