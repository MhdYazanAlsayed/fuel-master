using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Entities;

namespace FuelMaster.HeadOffice.Core.Interfaces.Authentication
{
    public interface ISigninService : IScopedDependency
    {
        Task<FuelMasterUser?> GetLoggedUserAsync(bool includeEmployee = false);
        string? GetLoggedUserId();
        Task<int?> TryToGetStationIdAsync();
    }
}
