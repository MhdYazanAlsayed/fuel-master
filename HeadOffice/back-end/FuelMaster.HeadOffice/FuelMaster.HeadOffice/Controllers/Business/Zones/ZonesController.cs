using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.ZoneService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core.Zones;
using FuelMaster.HeadOffice.Controllers.Zones.Validators;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Helpers;
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
        public async Task<IActionResult> GetPagination([FromQuery, BindRequired] int page)
        {
            try
            {
                var result = await _zoneService.GetPaginationAsync(page);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ZoneDto zoneDto)
        {
            var validator = new ZoneDtoValidator();
            var validationResult = validator.Validate(zoneDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var result = await _zoneService.CreateAsync(zoneDto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] ZoneDto zoneDto)
        {
            var validator = new ZoneDtoValidator();
            var validationResult = validator.Validate(zoneDto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var result = await _zoneService.UpdateAsync(id, zoneDto);
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
