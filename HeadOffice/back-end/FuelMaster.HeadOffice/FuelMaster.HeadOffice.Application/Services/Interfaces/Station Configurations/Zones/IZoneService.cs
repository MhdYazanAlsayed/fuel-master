using System;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Zones.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Zones.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Core.Zones;

public interface IZoneService : 
IDefaultBusinessQueryService<ZoneResult>, 
IBusinessService<ZoneDto, ZoneResult>, IScopedDependency
{
    Task<List<Zone>> GetCachedZonesAsync();
    Task<IEnumerable<ZonePriceResult>> GetPricesAsync(int zoneId);
    Task<ResultDto> ChangePriceAsync(int zoneId, ChangePriceDto dto);
}
