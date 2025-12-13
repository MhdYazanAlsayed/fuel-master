using FuelMaster.HeadOffice.Application.Services.Implementations.Business.FuelMasterRoleService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Controllers.FuelMasterRoles.Validators;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Controllers;

[Route("api/fuel-master-roles")]
public class FuelMasterRolesController : FuelMasterController
{
    private readonly IFuelMasterRoleService _roleService;

    public FuelMasterRolesController(IFuelMasterRoleService roleService)
    {
        _roleService = roleService;
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
        catch (DbUpdateException)
        {
            return BadRequest(Resource.CantDelete);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

