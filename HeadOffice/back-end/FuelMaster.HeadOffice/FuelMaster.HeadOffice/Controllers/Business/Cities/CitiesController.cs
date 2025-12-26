
using FuelMaster.HeadOffice.Application.Services.Interfaces.Cities.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core;
using FuelMaster.HeadOffice.Controllers.Cities.Validators;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FuelMaster.HeadOffice.Controllers
{
    [Route("api/cities")]
    public class CitiesController : FuelMasterController
    {
        private readonly ICityService _cityRepository;

        public CitiesController(ICityService cityService)
        {
            _cityRepository = cityService;
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPaginationAsync([FromQuery] int page)
        {
            try
            {
                var cities = await _cityRepository.GetPaginationAsync(page);
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var cities = await _cityRepository.GetAllAsync();
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CityDto dto)
        {
			var validator = new CityDtoValidator();
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
                var result = await _cityRepository.CreateAsync(dto);
                if (!result.Succeeded) return BadRequest(result.Message);

                return Ok(result.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(int id, [FromBody] CityDto dto)
        {
			var validator = new CityDtoValidator();
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
                var result = await _cityRepository.UpdateAsync(id, dto);
                if (!result.Succeeded) return BadRequest(result.Message);

                return Ok(result.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            try
            {
                var result = await _cityRepository.DetailsAsync(id);
                if (result is null) return NotFound();

                return Ok(result);
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
                var result = await _cityRepository.DeleteAsync(id);
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
}
