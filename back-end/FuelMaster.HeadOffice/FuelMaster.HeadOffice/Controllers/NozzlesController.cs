//using FuelMaster.HeadOffice.Core.Contracts.Entities;
//using FuelMaster.HeadOffice.Core.Helpers;
//using FuelMaster.HeadOffice.Core.Models.Requests.Nozzles;
//using FuelMaster.HeadOffice.Core.Resources;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace FuelMaster.HeadOffice.Controllers
//{
//    [ApiController]
//    [Route("api/nozzles")]
//    public class NozzlesController : FuelMasterController
//    {
//        private readonly INozzleService _nozzleService;

//        public NozzlesController(INozzleService nozzleService)
//        {
//            _nozzleService = nozzleService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll([FromQuery] GetNozzleDto dto)
//        {
//            var nozzles = await _nozzleService.GetAllAsync(dto);
//            return Ok(nozzles);
//        }

//        [HttpGet("pagination")]
//        public async Task<IActionResult> GetPaginationAsync([FromQuery] GetNozzlePaginationDto dto)
//        {
//            var nozzles = await _nozzleService.GetPaginationAsync(dto);
//            return Ok(nozzles);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] NozzleDto nozzleDto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var result = await _nozzleService.CreateAsync(nozzleDto);
//            if (!result.Succeeded) return BadRequest(result.Message);

//            return Ok(result.Entity);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Edit(int id, [FromBody] NozzleDto nozzleDto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var result = await _nozzleService.EditAsync(id, nozzleDto);
//            if (!result.Succeeded) return BadRequest(result.Message);

//            return Ok(result.Entity);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> Details(int id)
//        {
//            var nozzle = await _nozzleService.DetailsAsync(id);
//            if (nozzle == null)
//            {
//                return NotFound();
//            }

//            return Ok(nozzle);
//        }

//        [HttpDelete("{id}")]
//        public async Task<ActionResult> Delete(int id)
//        {
//            try 
//            {
//                var result = await _nozzleService.DeleteAsync(id);
//                if (!result.Succeeded) return BadRequest(result.Message);
//            }
//            catch (DbUpdateException)
//            {
//                return BadRequest(Resource.CantDelete);
//            }

//            return Ok();
//        }
//    }

//}
