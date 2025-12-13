using System;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface INozzleRepository : IRepository<Nozzle>, IScopedDependency
{
    Task<List<Nozzle>> GetAllAsync(bool includeTank = false, bool includePump = false, bool includeFuelType = false);
    Task<(List<Nozzle>, int)> GetPaginationAsync(int page, int pageSize, bool includeTank = false, bool includePump = false, bool includeFuelType = false);
}

