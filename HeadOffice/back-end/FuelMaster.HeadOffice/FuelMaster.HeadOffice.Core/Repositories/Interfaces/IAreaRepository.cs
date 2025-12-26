using FuelMaster.HeadOffice.Core.Entities.Configs.Area;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface IAreaRepository : IRepository<Area>, IScopedDependency
{
    Task<Area?> DetailsAsync(int id, bool includeStations = false);
    Task<List<Area>> GetAllAsync(bool includeStations = false, bool includeCity = false);
    Task<(List<Area>, int)> GetPaginationAsync(int page, int pageSize, bool includeStations = false, bool includeCity = false);
}

