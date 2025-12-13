using FuelMaster.HeadOffice.Application.Services.Interfaces.Database;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;

namespace FuelMaster.HeadOffice.Helpers;

public class Tenants : ITenants
{
    private static readonly Dictionary<Guid, TenantConfig> _tenants = new();
    private static readonly Dictionary<string, TenantConfig> _tenantsByName = new();

    private readonly IFuelMasterAPI _fuelMasterAPI;
    private readonly IConnectionString _connectionStringService;
    public Tenants(IFuelMasterAPI fuelMasterAPI, IConnectionString connectionStringService)
    {
        _fuelMasterAPI = fuelMasterAPI;
        _connectionStringService = connectionStringService;
    }


    public async Task<List<TenantConfig>> GetAllTenantsAsync()
    {
        var tenants = await _fuelMasterAPI.GetTenantsAsync();

        var mappedData = tenants.Select(x =>
        {
            var connectionString = _connectionStringService.GetConnectionString(x.DatabaseName);
            return new TenantConfig() 
            { 
                TenantId = x.TenantId, 
                ConnectionString = connectionString, 
                IsActive = x.IsActive,
                TenantName = x.TenantName
            };
        }).ToList();

        UpdateTenants(mappedData);

        return mappedData;
    }

    public async Task<TenantConfig?> GetTenantAsync(Guid tenantId)
    {
        var tenant = _tenants.TryGetValue(tenantId, out var result) ? result : null;
        if (tenant is null || !tenant.IsActive)
        {
            /*
                We get all tenants to check for these reasons : 
                1 - if it was null, we'll check if user already registered that tenant.
                2 - if it was not active, maybe user updated the subscription.
                To make sure that the tenant has been updated or not.
            */
            await GetAllTenantsAsync();
            tenant = _tenants.TryGetValue(tenantId, out tenant) ? tenant : null;
        }
        return tenant;
    }

    public async Task<TenantConfig?> GetTenantByNameAsync(string tenantName)
    {
        var tenant = _tenantsByName.TryGetValue(tenantName, out var result) ? result : null;
        if (tenant is null || !tenant.IsActive)
        {
            await GetAllTenantsAsync();
            tenant = _tenantsByName.TryGetValue(tenantName, out tenant) ? tenant : null;
        }
        return tenant;
    }
    // <summary>
    /// This private method is used to update the tenants in the memory.
    /// It's a private method and should be called only from the this class.
    /// No one supposed to call this method from outside of this class.
    /// It's also used when app runs for the first time to get the tenants from the FuelMasterAPI and store them in the memory.
    /// </summary>
    /// <param name="tenants"></param>
    private void UpdateTenants (List<TenantConfig> tenants)
    {
        _tenants.Clear();
        _tenantsByName.Clear();
        foreach (var tenant in tenants)
        {
            _tenants.Add(tenant.TenantId, tenant);
            _tenantsByName.Add(tenant.TenantName, tenant);
        }
    }
}