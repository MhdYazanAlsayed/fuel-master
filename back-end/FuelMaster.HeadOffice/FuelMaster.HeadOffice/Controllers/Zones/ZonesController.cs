using FuelMaster.HeadOffice.Core.Contracts.Entities.Zones;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Zones;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Controllers.Zones
{
    [Route("api/zones")]
    public class ZonesController : FuelMasterController
    {
        private readonly IZoneService _zoneService;

        public ZonesController(IZoneService zoneService)
        {
            _zoneService = zoneService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zone>>> GetAll()
        {
            var zones = await _zoneService.GetAllAsync();
            return Ok(zones);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<PaginationDto<Zone>>> GetPagination([FromQuery , BindRequired] int page)
        {
            var result = await _zoneService.GetPaginationAsync(page);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ZoneDto zoneDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _zoneService.CreateAsync(zoneDto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] ZoneDto zoneDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _zoneService.EditAsync(id, zoneDto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Zone>> Details(int id)
        {
            var zone = await _zoneService.DetailsAsync(id);
            if (zone == null)
            {
                return NotFound();
            }

            return Ok(zone);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try 
            {
                var result = await _zoneService.DeleteAsync(id);
                if (!result.Succeeded) return BadRequest(result.Message);
            }
            catch (DbUpdateException)
            {
                return BadRequest(Resource.CantDelete);
            }

            return Ok();
        }
    }
}
