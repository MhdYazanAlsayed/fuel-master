using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Infrastructure.Services.Implementations.External.Responses;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;

public interface IFuelMasterAPI : IScopedDependency
{
    /// <summary>
    /// This method is used to get all tenants from the FuelMasterAPI.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="Exception">Thrown when the request fails.</exception>
    Task<List<TenantConfigResponse>> GetTenantsAsync();
}