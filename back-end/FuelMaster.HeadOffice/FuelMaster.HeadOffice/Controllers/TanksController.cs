using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Requests.Tanks;
using FuelMaster.HeadOffice.Core.Models.Responses.Tanks;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Controllers
{
    [Route("api/tanks")]
    public class TanksController: FuelMasterController
    {
        private readonly ITankService _tankService;

        public TanksController(ITankService tankService)
        {
            _tankService = tankService;
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPaginationAsync([FromQuery , BindRequired] int page , [FromQuery] GetTankRequest dto)
        {
            return Ok(await _tankService.GetPaginationAsync(page, dto));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetTankRequest dto)
        {
            var tanks = await _tankService.GetAllAsync(dto);
            return Ok(tanks);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TankRequest tankDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _tankService.CreateAsync(tankDto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, TankRequest tankDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _tankService.EditAsync(id, tankDto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok(result.Entity);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tank>> Details(int id)
        {
            var tank = await _tankService.DetailsAsync(id);
            if (tank == null)
            {
                return NotFound();
            }

            return Ok(tank);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try 
            {
                var result = await _tankService.DeleteAsync(id);
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
