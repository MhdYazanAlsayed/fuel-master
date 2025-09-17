using FuelMaster.HeadOffice.Core.Contracts.Entities.Zones;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Requests.Zones;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers.Zones
{
    [Route("api/zones")]
    public class ZonePricesController(IZoneService _zoneService) : FuelMasterController
    {
        [HttpGet("{zoneId}/prices")]
        public async Task<IActionResult> GetPricesAsync (int zoneId)
        {
            return Ok(await _zoneService.GetPricesAsync(zoneId));
        }

        [HttpPut("{zoneId}/prices")]
        public async Task<IActionResult> ChangePriceAsync (int zoneId , [FromBody] ChangePriceDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _zoneService.ChangePriceAsync(zoneId, dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok();
        
        }
    }
}
