using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Entities;

namespace FuelMaster.HeadOffice.Core.Contracts.Authentication
{
    public interface ISigninService : IScopedDependency
    {
        Task<FuelMasterUser?> GetLoggedUserAsync(bool includeEmployee = false);
        string? GetLoggedUserId();
        Task<int?> TryToGetStationIdAsync();
    }
}
