using System;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface IStationRepository : IRepository<Station>, IScopedDependency
{
    Task<Station?> DetailsAsync(int id, bool includeCity = false, bool includeZone = false, bool includeArea = false);
    Task<List<Station>> GetAllAsync(bool includeCity = false, bool includeZone = false, bool includeArea = false);
    Task<(List<Station>, int)> GetPaginationAsync(int currentPage, int pageSize, bool includeCity = false, bool includeZone = false, bool includeArea = false);
}
