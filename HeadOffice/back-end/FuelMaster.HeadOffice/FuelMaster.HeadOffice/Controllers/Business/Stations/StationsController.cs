using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Controllers.Stations.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using FuelMaster.HeadOffice.Helpers;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.StationService.DTOs;

namespace FuelMaster.HeadOffice.Controllers
{
    [Route("api/stations")]
    public class StationsController: FuelMasterController
    {
        private readonly IStationService _stationService;
        private readonly ILogger<StationsController> _logger;

        public StationsController(IStationService stationService, ILogger<StationsController> logger)
        {
            _stationService = stationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try 
            {
                var stations = await _stationService.GetAllAsync();

                return Ok(stations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination([FromQuery , BindRequired] int page)
        {
            try 
            {
                var result = await _stationService.GetPaginationAsync(page);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting pagination stations: {ErrorMessage}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateStationDto dto)
        {
            var validator = new StationDtoValidator();
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
                var result = await _stationService.CreateAsync(dto);
                if (!result.Succeeded) return BadRequest(result.Message);

                return Ok(result.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] EditStationDto dto)
        {
            var validator = new EditStationDtoValidator();
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
                var result = await _stationService.UpdateAsync(id, dto);
                if (!result.Succeeded) return BadRequest(result.Message);

                return Ok(result.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Station>> Details(int id)
        {
            try 
            {
                var result = await _stationService.DetailsAsync(id);
                if (result is null) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try 
            {
                var result = await _stationService.DeleteAsync(id);
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
