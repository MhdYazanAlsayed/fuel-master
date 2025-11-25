using System;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface IStationRepository : IRepository<Station>
{
    Task<List<Station>> GetAllAsync(bool includeCity = false, bool includeZone = false);
    Task<(List<Station>, int)> GetPaginationAsync(int currentPage, int pageSize, bool includeCity = false, bool includeZone = false);
}
