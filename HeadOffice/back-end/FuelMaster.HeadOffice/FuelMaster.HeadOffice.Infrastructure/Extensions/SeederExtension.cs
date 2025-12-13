using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FuelMaster.HeadOffice.Infrastructure.Extensions;

public static class SeederExtension
{
    public static async Task<WebApplication> UseSeedersAsync (this WebApplication app)
    {
        var scope = app.Services.CreateScope();

        var seederDiscoveryService = scope.ServiceProvider.GetService<ISeederDiscovery>();
        
        if (seederDiscoveryService is null)
            throw new NullReferenceException("SeederDiscoveryService was not registered");

        await seederDiscoveryService.ExecuteAllSeedersAsync();

        return app;
    }
}