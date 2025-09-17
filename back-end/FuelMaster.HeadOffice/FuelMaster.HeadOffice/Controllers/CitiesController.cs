using AutoMapper.Features;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Cities;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Controllers
{
    [Route("api/cities")]
    public class CitiesController : FuelMasterController
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<PaginationDto<City>>> GetPaginationAsync([FromQuery] int page)
        {
            var cities = await _cityService.GetPaginationAsync(page);
            return Ok(cities);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetAllAsync ()
        {
            var cities = await _cityService.GetAllAsync();
            return Ok(cities);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] CityDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _cityService.CreateAsync(dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditAsync(int id , [FromBody] CityDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _cityService.EditAsync(id , dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> DetailsAsync(int id)
        {
            return Ok(await _cityService.DetailsAsync(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try 
            {
                var result = await _cityService.DeleteAsync(id);
                if (!result.Succeeded) return BadRequest(result.Message);

                return Ok();
            }
            catch (DbUpdateException)
            {
                return BadRequest(Resource.CantDelete);
            }
        }
    }
}
