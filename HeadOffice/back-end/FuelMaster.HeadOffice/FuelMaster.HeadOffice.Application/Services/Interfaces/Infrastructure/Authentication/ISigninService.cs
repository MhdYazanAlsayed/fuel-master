using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Entities;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication
{
    public interface ISigninService : IScopedDependency
    {
        Task<FuelMasterUser?> GetCurrentUserAsync(bool includeEmployee = false, bool includeAreaOfAccess = false);
        string? GetCurrentUserId();
        int? GetCurrentEmployeeId();
        int? GetCurrentStationId();
        int? GetCurrentCityId();
        int? GetCurrentAreaId();
        Scope? GetCurrentScope();
    }
}
