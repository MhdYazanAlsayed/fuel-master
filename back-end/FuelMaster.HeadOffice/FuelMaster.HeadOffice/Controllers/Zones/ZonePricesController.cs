using FuelMaster.HeadOffice.Core.Interfaces.Entities.Zones;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Zones.Dtos;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Controllers.Zones.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers.Zones
{
   [Route("api/zones")]
   public class ZonePricesController(IZoneRepository _zoneService) : FuelMasterController
   {
       [HttpGet("{zoneId}/prices")]
       public async Task<IActionResult> GetPricesAsync (int zoneId)
       {
           return Ok(await _zoneService.GetPricesAsync(zoneId));
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
