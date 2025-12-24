using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.DTOs;
using FuelMaster.HeadOffice.Controllers.Employees.Validators;
using FuelMaster.HeadOffice.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FuelMaster.HeadOffice.Controllers;

[Route("api/employees")]
public class EmployeeController : FuelMasterController
{
    private readonly IEmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateEmployeeDto dto)
    {
        var validator = new CreateEmployeeDtoValidator();
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
            var result = await _employeeService.CreateAsync(dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> DetailsAsync(int id)
    {
        try
        {
            var employee = await _employeeService.DetailsAsync(id);
            if (employee is null) return NotFound();

            return Ok(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employee details with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateEmployeeDto dto)
    {
        var validator = new UpdateEmployeeDtoValidator();
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
            var result = await _employeeService.UpdateAsync(id, dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee with ID: {Id}", id);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> GetPaginationAsync([FromQuery, BindRequired] int page)
    {
        try
        {
            var result = await _employeeService.GetPaginationAsync(page);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated employees for page {Page}", page);
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var employees = await _employeeService.GetAllAsync();
            return Ok(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all employees");
            return BadRequest(ex.Message);
        }
    }
}
