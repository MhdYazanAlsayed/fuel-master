using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Entities;

namespace FuelMaster.HeadOffice.Core.Contracts.Authentication
{
    public interface IAuthorization : IScopedDependency
    {
        Task<FuelMasterUser?> GetLoggedUserAsync(bool includeEmployee = false);
        Task<string?> GetLoggedUserIdAsync();
        Task<int?> TryToGetStationIdAsync();
    }
}
