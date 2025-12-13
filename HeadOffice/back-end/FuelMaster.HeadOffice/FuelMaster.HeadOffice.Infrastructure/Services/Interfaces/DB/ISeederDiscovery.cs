using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Interfaces.DB;

public interface ISeederDiscovery : IScopedDependency
{
    /// <summary>
    /// This method is used to execute all seeders for all tenants.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ExecuteAllSeedersAsync();
}