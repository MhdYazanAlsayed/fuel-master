using FuelMaster.HeadOffice.Controllers.Pumps.Validators;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Pumps;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Pumps.Dtos;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Pumps.Results;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Controllers
{
    [Route("api/pumps")]
    public class PumpsController : FuelMasterController
    {
        private readonly IPumpRepository _pumpRepository;
        private readonly ILogger<PumpsController> _logger;

        public PumpsController(IPumpRepository pumpRepository, ILogger<PumpsController> logger)
        {
            _pumpRepository = pumpRepository;
            _logger = logger;
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination([FromQuery, BindRequired] int page, [FromQuery] GetPumpRequest dto)
        {
            try
            {
                var result = await _pumpRepository.GetPaginationAsync(page, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paginated pumps for page {Page}", page);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetPumpRequest dto)
        {
            try
            {
                var pumps = await _pumpRepository.GetAllAsync(dto);
                return Ok(pumps);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all pumps");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreatePumpDto pumpDto)
        {
            var validator = new CreatePumpDtoValidator();
            var validationResult = validator.Validate(pumpDto);
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
                var result = await _pumpRepository.CreateAsync(pumpDto);
                if (!result.Succeeded) return BadRequest(result.Message);

                return Ok(result.Entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error creating pump");
                return BadRequest(Resource.SthWentWrong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating pump");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] EditPumpDto pumpDto)
        {
            var validator = new EditPumpDtoValidator();
            var validationResult = validator.Validate(pumpDto);
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
                var result = await _pumpRepository.EditAsync(id, pumpDto);
                if (!result.Succeeded) return BadRequest(result.Message);

                return Ok(result.Entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error editing pump with ID {Id}", id);
                return BadRequest(Resource.SthWentWrong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing pump with ID {Id}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PumpResult>> Details(int id)
        {
            try
            {
                var pump = await _pumpRepository.DetailsAsync(id);
                if (pump == null)
                {
                    return NotFound();
                }

                return Ok(pump);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pump details for ID {Id}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try 
            {
                var result = await _pumpRepository.DeleteAsync(id);
                if (!result.Succeeded) return BadRequest(result.Message);

                return Ok();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error deleting pump with ID {Id}", id);
                return BadRequest(Resource.CantDelete);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting pump with ID {Id}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}
