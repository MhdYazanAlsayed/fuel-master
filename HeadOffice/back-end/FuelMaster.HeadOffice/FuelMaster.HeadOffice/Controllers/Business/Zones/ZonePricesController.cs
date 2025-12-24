using FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core.Zones;
using FuelMaster.HeadOffice.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers.Zones
{
  [Route("api/zones")]
  public class ZonePricesController(IZoneService _zoneService) : FuelMasterController
  {
      [HttpGet("{zoneId}/prices")]
      public async Task<IActionResult> GetPricesAsync (int zoneId)
      {
        try
        {
            return Ok(await _zoneService.GetPricesAsync(zoneId));

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
      }

      [HttpPut("{zoneId}/prices")]
      public async Task<IActionResult> ChangePriceAsync (int zoneId , [FromBody] ChangePriceDto dto)
      {
          var result = await _zoneService.ChangePriceAsync(zoneId, dto);
          if (!result.Succeeded) return BadRequest(result.Message);

          return Ok();
      }
  }
}
