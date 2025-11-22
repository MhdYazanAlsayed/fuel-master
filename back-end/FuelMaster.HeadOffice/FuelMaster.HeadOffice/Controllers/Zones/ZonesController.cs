using FuelMaster.HeadOffice.Core.Interfaces.Entities.Zones;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Zones.Dtos;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Controllers.Zones.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Controllers.Zones
{
   [Route("api/zones")]
   public class ZonesController : FuelMasterController
   {
       private readonly IZoneRepository _zoneService;

       public ZonesController(IZoneRepository zoneService)
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
