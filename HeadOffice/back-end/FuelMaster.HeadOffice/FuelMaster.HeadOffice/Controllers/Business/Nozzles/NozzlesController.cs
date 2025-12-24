using FuelMaster.HeadOffice.Controllers.Nozzles.Validators;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using FuelMaster.HeadOffice.Helpers;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService.DTOs;

namespace FuelMaster.HeadOffice.Controllers
{
    [Route("api/nozzles")]
    public class NozzlesController : FuelMasterController
    {
        private readonly INozzleService _nozzleService;
        private readonly ILogger<NozzlesController> _logger;

        public NozzlesController(INozzleService nozzleService, ILogger<NozzlesController> logger)
        {
            _nozzleService = nozzleService;
            _logger = logger;
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination([FromQuery, BindRequired] int page, [FromQuery] GetNozzleDto dto)
        {
            try
            {
                var result = await _nozzleService.GetPaginationAsync(page, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paginated nozzles for page {Page}", page);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetNozzleDto dto)
        {
            try
            {
                var nozzles = await _nozzleService.GetAllAsync(dto);
                return Ok(nozzles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all nozzles");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] NozzleDto nozzleDto)
        {
            var validator = new NozzleDtoValidator();
            var validationResult = validator.Validate(nozzleDto);
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
                var result = await _nozzleService.CreateAsync(nozzleDto);
                if (!result.Succeeded) return BadRequest(result.Message);

                return Ok(result.Entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error creating nozzle");
                return BadRequest(Resource.SthWentWrong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating nozzle");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] UpdateNozzleDto nozzleDto)
        {
            var validator = new UpdateNozzleDtoValidator();
            var validationResult = validator.Validate(nozzleDto);
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
                var result = await _nozzleService.UpdateAsync(id, nozzleDto);
                if (!result.Succeeded) return BadRequest(result.Message);

                return Ok(result.Entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error editing nozzle with ID {Id}", id);
                return BadRequest(Resource.SthWentWrong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing nozzle with ID {Id}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NozzleDto>> Details(int id)
        {
            try
            {
                var nozzle = await _nozzleService.DetailsAsync(id);
                if (nozzle == null)
                {
                    return NotFound();
                }

                return Ok(nozzle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting nozzle details for ID {Id}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try 
            {
                var result = await _nozzleService.DeleteAsync(id);
                if (!result.Succeeded) return BadRequest(result.Message);

                return Ok();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error deleting nozzle with ID {Id}", id);
                return BadRequest(Resource.CantDelete);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting nozzle with ID {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
