using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FuelMaster.HeadOffice.Infrastructure.Extensions;

/// <summary>
/// This extension is used to get all tenants from the FuelMasterAPI and store them into the ITenants service.
/// It's a singleton service.
/// It will be injected in Dependencies class.
/// </summary>
public static class TenantsExtension
{
    public static async Task<WebApplication> GetTenantsFromFuelMasterAPIAsync(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var tenantsService = scope.ServiceProvider.GetService<ITenants>();

        if (tenantsService is null)
            throw new NullReferenceException("ITenants service was not registered");

        await tenantsService.GetAllTenantsAsync();

        return app;
    }
}