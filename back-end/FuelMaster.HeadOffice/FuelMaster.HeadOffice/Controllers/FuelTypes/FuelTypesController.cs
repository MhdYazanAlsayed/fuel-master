using FuelMaster.HeadOffice.Core.Interfaces.Repositories.FuelTypes;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.FuelTypes.Dtos;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Controllers.FuelTypes.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FuelMaster.HeadOffice.Controllers
{
    [Route("api/fuel-types")]
    [ApiController]
    public class FuelTypesController : FuelMasterController
    {
        private readonly IFuelTypeRepository _fuelTypeRepository;
        
        public FuelTypesController(IFuelTypeRepository fuelTypeRepository)
        {
            _fuelTypeRepository = fuelTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fuelTypes = await _fuelTypeRepository.GetAllAsync();
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

            var result = await _fuelTypeRepository.CreateAsync(dto);
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

            var result = await _fuelTypeRepository.UpdateAsync(id, dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }
    }
}
