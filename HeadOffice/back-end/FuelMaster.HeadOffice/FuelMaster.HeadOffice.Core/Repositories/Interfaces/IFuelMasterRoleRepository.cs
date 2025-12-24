using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface IFuelMasterRoleRepository : IRepository<FuelMasterRole>, IScopedDependency
{
    Task<FuelMasterRole?> DetailsAsync(int id, bool includeAreasOfAccess = false);
    Task<List<FuelMasterRole>> GetAllAsync(bool includeAreasOfAccess = false);
    Task<(List<FuelMasterRole> Data, int TotalCount)> GetPaginationAsync(int page, int pageSize, bool includeAreasOfAccess = false);
}

