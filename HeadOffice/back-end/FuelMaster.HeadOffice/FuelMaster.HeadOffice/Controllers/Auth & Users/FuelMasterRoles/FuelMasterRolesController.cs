using FuelMaster.HeadOffice.Application.Services.Implementations.Business.FuelMasterRoleService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Controllers.FuelMasterRoles.Validators;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Controllers;

[Route("api/fuel-master-roles")]
public class FuelMasterRolesController : FuelMasterController
{
    private readonly IFuelMasterRoleService _roleService;
    private readonly ILogger<FuelMasterRolesController> _logger;

    public FuelMasterRolesController(IFuelMasterRoleService roleService, ILogger<FuelMasterRolesController> logger)
    {
        _roleService = roleService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all fuel master roles");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> GetPaginationAsync([FromQuery, BindRequired] int page)
    {
        try
        {
            var result = await _roleService.GetPaginationAsync(page);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated fuel master roles for page {Page}", page);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> DetailsAsync(int id)
    {
        try
        {
            var result = await _roleService.DetailsAsync(id);
            if (result is null) return NotFound();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting fuel master role details with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] FuelMasterRoleDto dto)
    {
        var validator = new FuelMasterRoleDtoValidator();
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
            var result = await _roleService.CreateAsync(dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating fuel master role");
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] FuelMasterRoleDto dto)
    {
        var validator = new FuelMasterRoleDtoValidator();
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
            var result = await _roleService.UpdateAsync(id, dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating fuel master role with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            var result = await _roleService.DeleteAsync(id);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok();
        }
        catch (NotImplementedException)
        {
            _logger.LogWarning("DeleteAsync is not yet implemented for fuel master roles");
            return BadRequest("Delete operation is not yet implemented");
        }
        catch (DbUpdateException)
        {
            _logger.LogError("Database error deleting fuel master role with ID: {Id}", id);
            return BadRequest(Resource.CantDelete);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting fuel master role with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }
}

