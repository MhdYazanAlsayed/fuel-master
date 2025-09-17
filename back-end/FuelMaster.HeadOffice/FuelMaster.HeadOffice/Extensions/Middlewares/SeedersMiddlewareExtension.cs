using FuelMaster.HeadOffice.Core.Contracts.Database;

namespace FuelMaster.HeadOffice.Extensions.Middlewares
{
    public static class SeedersMiddlewareExtension
    {
        public static async Task<WebApplication> UseSeedersAsync (this WebApplication app)
        {
            var scop = app.Services.CreateScope();
            var seederService = scop.ServiceProvider.GetService<ISeeder>();
            if (seederService is null)
                throw new NullReferenceException();

            await seederService.SeedAsync();

            return app;
        }
    }
}
