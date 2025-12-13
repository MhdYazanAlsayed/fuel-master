using FuelMaster.HeadOffice.Application.Services.Implementations.Tanents.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;
using FuelMaster.HeadOffice.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers.Tanents
{
    [Route("api/tenants")]
    public class TanentsController : FuelMasterController
    {
        private readonly IMultiTenancyManager _multiTenancyManager;
        public TanentsController(IMultiTenancyManager multiTenancyManager)
        {
            _multiTenancyManager = multiTenancyManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateDatabaseForTenantAsync([FromBody] CreateDatabaseForTanentDto createDatabaseForTanentDto)
        {
            var result = await _multiTenancyManager.CreateDatabaseForTenantAsync(createDatabaseForTanentDto);
            if (!result.Succeeded) 
                return BadRequest(new 
                {
                    Success = false,
                    ErrorMessage = result.Message,
                });

            return Ok(new 
            {
                Success = true,
                DatabaseName = result.Entity?.DatabaseName,
                ConnectionString = result.Entity?.ConnectionString,
            });
        }

       
    }
}
