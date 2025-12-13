using System;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Core.Zones;

public interface IZoneService : IBusinessService<ZoneDto, ZoneResult>, IScopedDependency
{
    Task<List<Zone>> GetCachedZonesAsync();
    Task<IEnumerable<ZonePriceResult>> GetPricesAsync(int zoneId);
    Task<ResultDto> ChangePriceAsync(int zoneId, ChangePriceDto dto);
}
