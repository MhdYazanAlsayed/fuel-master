using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using FuelMaster.HeadOffice.Helpers;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.TankService.DTOs;
using FuelMaster.HeadOffice.Controllers.Tanks.Validators;

namespace FuelMaster.HeadOffice.Controllers
{
  [Route("api/tanks")]
  public class TanksController: FuelMasterController
  {
      private readonly ITankService _tankService;
      private readonly ILogger<TanksController> _logger;

      public TanksController(ITankService tankService, ILogger<TanksController> logger)
      {
          _tankService = tankService;
          _logger = logger;
      }

      [HttpGet("pagination")]
      public async Task<IActionResult> GetPaginationAsync
      ([FromQuery , BindRequired] int page , [FromQuery] GetTankDto dto)
      {
          try
          {
              var result = await _tankService.GetPaginationAsync(page, dto);
              return Ok(result);
          }
          catch (Exception ex)
          {
              _logger.LogError(ex, "Error getting paginated tanks for page {Page}", page);
              return BadRequest(ex.Message);
          }
      }

      [HttpGet]
      public async Task<IActionResult> GetAll([FromQuery] GetTankDto dto)
      {
          try
          {
              var tanks = await _tankService.GetAllAsync(dto);
              return Ok(tanks);
          }
          catch (Exception ex)
          {
              _logger.LogError(ex, "Error getting all tanks");
              return BadRequest(ex.Message);
          }
      }

      [HttpPost]
      public async Task<ActionResult> Create([FromBody] TankDto dto)
      {
          var validator = new TankDtoValidator();
          var validationResult = validator.Validate(dto);
          if (!validationResult.IsValid)
          {
              foreach (var error in validationResult.Errors)
              {
                  ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
              }
              return BadRequest(ModelState);
          }

          try
          {
              var result = await _tankService.CreateAsync(dto);
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
      public async Task<ActionResult> Edit(int id, [FromBody] EditTankDto dto)
      {
          var validator = new TankEditDtoValidator();
          var validationResult = validator.Validate(dto);
          if (!validationResult.IsValid)
          {
              foreach (var error in validationResult.Errors)
              {
                  ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
              }
              return BadRequest(ModelState);
          }

          try
          {
              var result = await _tankService.UpdateAsync(id, dto);
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
              var tank = await _tankService.DetailsAsync(id);
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
              var result = await _tankService.DeleteAsync(id);
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
