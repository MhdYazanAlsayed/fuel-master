using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces.Zones;

public interface IZonePriceHistoryRepository : IScopedDependency
{
    Task<List<ZonePriceHistory>> GetAllAsync(int zonePriceId);
    Task<(List<ZonePriceHistory>, int)> GetPaginationAsync(int zonePriceId, int currentPage, int pageSize);
}
