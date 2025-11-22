using FuelMaster.HeadOffice.Core.Interfaces.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Tanks.Dtos;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Controllers
{
   [Route("api/tanks")]
   public class TanksController: FuelMasterController
   {
       private readonly ITankRepository _tankRepository;
       private readonly ILogger<TanksController> _logger;

       public TanksController(ITankRepository tankRepository, ILogger<TanksController> logger)
       {
           _tankRepository = tankRepository;
           _logger = logger;
       }

       [HttpGet("pagination")]
       public async Task<IActionResult> GetPaginationAsync([FromQuery , BindRequired] int page , [FromQuery] GetTankRequest dto)
       {
           try
           {
               var result = await _tankRepository.GetPaginationAsync(page, dto);
               return Ok(result);
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Error getting paginated tanks for page {Page}", page);
               return BadRequest(ex.Message);
           }
       }

       [HttpGet]
       public async Task<IActionResult> GetAll([FromQuery] GetTankRequest dto)
       {
           try
           {
               var tanks = await _tankRepository.GetAllAsync(dto);
               return Ok(tanks);
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Error getting all tanks");
               return BadRequest(ex.Message);
           }
       }

       [HttpPost]
       public async Task<ActionResult> Create([FromBody] CreateTankDto tankDto)
       {
           if (!ModelState.IsValid) return BadRequest(ModelState);

           try
           {
               var result = await _tankRepository.CreateAsync(tankDto);
               if (!result.Succeeded) return BadRequest(result.Message);

               return Ok(result.Entity);
           }
           catch (DbUpdateException ex)
           {
               _logger.LogError(ex, "Database error creating tank");
               return BadRequest(Resource.SthWentWrong);
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Error creating tank");
               return BadRequest(ex.Message);
           }
       }

       [HttpPut("{id}")]
       public async Task<ActionResult> Edit(int id, [FromBody] EditTankDto tankDto)
       {
           if (!ModelState.IsValid) return BadRequest(ModelState);

           try
           {
               var result = await _tankRepository.EditAsync(id, tankDto);
               if (!result.Succeeded) return BadRequest(result.Message);

               return Ok(result.Entity);
           }
           catch (DbUpdateException ex)
           {
               _logger.LogError(ex, "Database error editing tank with ID {Id}", id);
               return BadRequest(Resource.SthWentWrong);
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Error editing tank with ID {Id}", id);
               return BadRequest(ex.Message);
           }
       }

       [HttpGet("{id}")]
       public async Task<ActionResult<Tank>> Details(int id)
       {
           try
           {
               var tank = await _tankRepository.DetailsAsync(id);
               if (tank == null)
               {
                   return NotFound();
               }

               return Ok(tank);
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Error getting tank details for ID {Id}", id);
               return BadRequest(ex.Message);
           }
       }

       [HttpDelete("{id}")]
       public async Task<ActionResult> Delete(int id)
       {
           try 
           {
               var result = await _tankRepository.DeleteAsync(id);
               if (!result.Succeeded) return BadRequest(result.Message);

               return Ok();
           }
           catch (DbUpdateException ex)
           {
               _logger.LogError(ex, "Database error deleting tank with ID {Id}", id);
               return BadRequest(Resource.CantDelete);
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Error deleting tank with ID {Id}", id);
               return BadRequest(ex.Message);
           }
       }
   }
}
