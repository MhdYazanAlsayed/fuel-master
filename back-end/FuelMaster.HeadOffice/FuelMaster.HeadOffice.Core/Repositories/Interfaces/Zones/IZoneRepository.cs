using System;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories;

public interface IZoneRepository : IRepository<Zone>, IScopedDependency
{
    Task<Zone?> DetailsAsync(int id, bool includePrices = false, bool includeFuelType = false, bool includeStations = false, bool includeTanks = false, bool includeNozzles = false);
    Task<List<Zone>> GetAllAsync(bool includePrices = false, bool includeFuelType = false, bool includeStations = false, bool includeTanks = false, bool includeNozzles = false);
    Task<(List<Zone>, int)> GetPaginationAsync(int page, int pageSize, bool includePrices = false, bool includeFuelType = false, bool includeStations = false, bool includeTanks = false, bool includeNozzles = false);
}
