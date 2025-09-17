using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Requests.Stations;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Controllers
{
    [Route("api/stations")]
    public class StationsController: FuelMasterController
    {
        private readonly IStationService _stationService;

        public StationsController(IStationService stationService )
        {
            _stationService = stationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Station>>> GetAll()
        {
            var stations = await _stationService.GetAllAsync();
            return Ok(stations);
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPagination([FromQuery , BindRequired] int page)
        {
            var result = await _stationService.GetPaginationAsync(page);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] StationRequest dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _stationService.CreateAsync(dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, StationRequest dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _stationService.EditAsync(id, dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Station>> Details(int id)
        {
            var station = await _stationService.DetailsAsync(id);
            if (station == null)
            {
                return NotFound();
            }

            return Ok(station);
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
