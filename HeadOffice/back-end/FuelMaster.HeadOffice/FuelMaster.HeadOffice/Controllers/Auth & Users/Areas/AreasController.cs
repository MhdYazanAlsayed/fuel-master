using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Area;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Areas.DTOs;
using FuelMaster.HeadOffice.Controllers.Areas.Validators;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Controllers;

[Route("api/areas")]
public class AreasController : FuelMasterController
{
    private readonly IAreaService _areaService;
    private readonly ILogger<AreasController> _logger;

    public AreasController(IAreaService areaService, ILogger<AreasController> logger)
    {
        _areaService = areaService;
        _logger = logger;
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> GetPaginationAsync([FromQuery] int page)
    {
        try
        {
            var areas = await _areaService.GetPaginationAsync(page);
            return Ok(areas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated areas for page {Page}", page);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var areas = await _areaService.GetAllAsync();
            return Ok(areas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all areas");
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] AreaDto dto)
    {
        var validator = new AreaDtoValidator();
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
            var result = await _areaService.CreateAsync(dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating area");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditAsync(int id, [FromBody] AreaDto dto)
    {
        var validator = new AreaDtoValidator();
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
            var result = await _areaService.UpdateAsync(id, dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating area with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> DetailsAsync(int id)
    {
        try
        {
            var result = await _areaService.DetailsAsync(id);
            if (result is null) return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting area details with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            var result = await _areaService.DeleteAsync(id);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok();
        }
        catch (DbUpdateException)
        {
            return BadRequest(Resource.CantDelete);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting area with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }
}

