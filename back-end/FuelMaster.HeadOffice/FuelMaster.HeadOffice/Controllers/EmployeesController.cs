using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Requests.Accounts;
using FuelMaster.HeadOffice.Core.Models.Requests.Employees;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers
{
    [Route("api/employees")]
    public class EmployeeController : FuelMasterController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAccountService _accountService;

        public EmployeeController(IEmployeeService employeeService , IAccountService accountService)
        {
            _employeeService = employeeService;
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _employeeService.CreateAsync(dto);
            if (!result.Succeeded) return BadRequest(ModelState);

            return Ok(result.Entity);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DetailsAsync (int id)
        {
            var employee = await _employeeService.DetailsAsync(id);
            if (employee is null) return NotFound();

            return Ok(employee);
        }

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> Edit(int employeeId, [FromBody] EditEmployeeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _employeeService.EditAsync(employeeId, dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination([FromQuery] GetPaginationEmployeeDto dto)
        {
            var result = await _employeeService.GetPaginationAsync(dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] int? stationId)
        {
            return Ok(await _employeeService.GetAsync(stationId));
        }

        [HttpPut("{employeeId}/edit-password")]
        public async Task<IActionResult> EditPasswordAsync(int employeeId , [FromBody] EditPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _employeeService.EditPasswordAsync(employeeId , dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok();
        }
    }

}
