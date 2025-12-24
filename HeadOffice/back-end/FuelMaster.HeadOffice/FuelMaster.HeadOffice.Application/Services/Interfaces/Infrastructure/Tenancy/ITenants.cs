using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;

/// <summary>
/// This interface is used to get all tenants from the memory.
/// It's a singleton service.
/// It will be injected in Dependencies class.
/// </summary>
public interface ITenants : IScopedDependency
{
    /// <summary>
    /// This method should call IFuelMasterAPI interface to get all and latest tenants. 
    /// It's almost used only in specific cases like apply migrations for all tenants.
    /// </summary>
    /// <returns>List<TenantConfig></returns>
    /// <exception cref="NotImplementedException"></exception>
    Task<List<TenantConfig>> GetAllTenantsAsync();

    /// <summary>
    /// This method is used to get the tenant from the memory by tenant name.
    /// If the tenant is not found in the memory or get it as not active, it will call the FuelMasterAPI to get the latest updated tenants.
    /// </summary>
    /// <param name="tenantName">The tenant name that came in request header or jwt claims.</param>
    /// <returns>TenantConfig? null if the tenant is not found in the memory even we got the latest updated tenants.</returns>
    Task<TenantConfig?> GetTenantByNameAsync(string tenantName);

    /// <summary>
    /// This method is used to get the tenant from the memory.
    /// If the tenant is not found in the memory or get it as not active, it will call the FuelMasterAPI to get the latest updated tenants.
    /// </summary>
    /// <param name="tenantId">The tenant id that came in request header or jwt claims.</param>
    /// <returns>TenantConfig? null if the tenant is not found in the memory even we got the latest updated tenants.</returns>

    Task<TenantConfig?> GetTenantAsync(Guid tenantId);
}