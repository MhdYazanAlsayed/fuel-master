using System;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface ITankRepository : IRepository<Tank>, IScopedDependency
{
    Task<List<Tank>> GetAllAsync(bool includeStation = false, bool includeFuelType = false, bool includeNozzles = false);
    Task<(List<Tank>, int)> GetPaginationAsync(int page, int pageSize, bool includeStation = false, bool includeFuelType = false, bool includeNozzles = false);
}
