using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface IFuelMasterAreaOfAccessRepository : IDefaultQuerableRepository<FuelMasterAreaOfAccess>, IRepository<FuelMasterAreaOfAccess>, IScopedDependency
{
    Task<FuelMasterAreaOfAccess?> DetailsAsync(int id);
}

