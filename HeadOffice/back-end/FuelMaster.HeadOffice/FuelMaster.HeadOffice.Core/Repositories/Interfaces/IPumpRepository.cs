using System;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface IPumpRepository : IRepository<Pump>, IScopedDependency
{
    Task<List<Pump>> GetAllAsync(bool includeStation = false, bool includeNozzles = false);
    Task<(List<Pump>, int)> GetPaginationAsync(int page, int pageSize, bool includeStation = false, bool includeNozzles = false);
}

