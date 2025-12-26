using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Zones.Results;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Zones;

public interface IZonePriceHistoryService : IScopedDependency
{
    Task<PaginationDto<ZonePriceHistoryPaginationResult>> GetPaginationAsync(int zonePriceId, int currentPage);
}