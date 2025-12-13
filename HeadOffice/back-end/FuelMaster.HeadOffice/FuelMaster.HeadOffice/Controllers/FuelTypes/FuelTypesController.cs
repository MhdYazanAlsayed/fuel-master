using FuelMaster.HeadOffice.Controllers.FuelTypes.Validators;
using Microsoft.AspNetCore.Mvc; 
using FuelMaster.HeadOffice.Helpers;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core;
using FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.DTOs;

namespace FuelMaster.HeadOffice.Controllers
{
    [Route("api/fuel-types")]
    [ApiController]
    public class FuelTypesController : FuelMasterController
    {
        private readonly IFuelTypeService _fuelTypeService;
        
        public FuelTypesController(IFuelTypeService fuelTypeRepository)
        {
            _fuelTypeService = fuelTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fuelTypes = await _fuelTypeService.GetAllAsync();
            return Ok(fuelTypes);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FuelTypeDto dto)
        {
            var validator = new FuelTypeDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            var result = await _fuelTypeService.CreateAsync(dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(int id, [FromBody] FuelTypeDto dto)
        {
            var validator = new FuelTypeDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = await _fuelTypeService.UpdateAsync(id, dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }
    }
}
