using FuelMaster.HeadOffice.Core.Interfaces.Entities.Zones;
using FuelMaster.HeadOffice.Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers.Zones
{
   [Route("api/zones/prices")]
   public class ZonePriceHistoriesController(IZoneRepository _zonePrice) : FuelMasterController
   {
       [HttpGet("{zonePriceId}/histories")]
       public async Task<IActionResult> GetZonePriceHistoriesAsync(int zonePriceId , [FromQuery] int page)
       {
           return Ok(await _zonePrice.GetHistoriesAsync(page, zonePriceId));
       }

   }
}
