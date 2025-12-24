using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Zones;
using FuelMaster.HeadOffice.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers.Zones
{
    [Route("api/zones/prices")]
    public class ZonePriceHistoriesController(IZonePriceHistoryService _zoneService) : FuelMasterController
    {
        [HttpGet("{zonePriceId}/histories")]
        public async Task<IActionResult> GetZonePriceHistoriesAsync(int zonePriceId, [FromQuery] int page)
        {
            return Ok(await _zoneService.GetPaginationAsync(zonePriceId , page));
        }
    }
}
