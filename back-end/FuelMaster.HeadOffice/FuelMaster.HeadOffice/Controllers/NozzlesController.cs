using FuelMaster.HeadOffice.Controllers.Nozzles.Validators;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Nozzles;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Nozzles.Dtos;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.Nozzles.Results;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Controllers
{
    [Route("api/nozzles")]
    public class NozzlesController : FuelMasterController
    {
        private readonly INozzleRepository _nozzleRepository;
        private readonly ILogger<NozzlesController> _logger;

        public NozzlesController(INozzleRepository nozzleRepository, ILogger<NozzlesController> logger)
        {
            _nozzleRepository = nozzleRepository;
            _logger = logger;
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination([FromQuery, BindRequired] int page, [FromQuery] GetNozzleRequest dto)
        {
            try
            {
                var result = await _nozzleRepository.GetPaginationAsync(page, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paginated nozzles for page {Page}", page);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetNozzleRequest dto)
        {
            try
            {
                var nozzles = await _nozzleRepository.GetAllAsync(dto);
                return Ok(nozzles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all nozzles");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateNozzleDto nozzleDto)
        {
            var validator = new CreateNozzleDtoValidator();
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
                var result = await _nozzleRepository.CreateAsync(nozzleDto);
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
        public async Task<ActionResult> Edit(int id, [FromBody] EditNozzleDto nozzleDto)
        {
            var validator = new EditNozzleDtoValidator();
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
                var result = await _nozzleRepository.EditAsync(id, nozzleDto);
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
        public async Task<ActionResult<NozzleResult>> Details(int id)
        {
            try
            {
                var nozzle = await _nozzleRepository.DetailsAsync(id);
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
                var result = await _nozzleRepository.DeleteAsync(id);
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
