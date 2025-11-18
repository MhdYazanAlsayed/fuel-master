//using FuelMaster.HeadOffice.Core.Contracts.Entities;
//using FuelMaster.HeadOffice.Core.Entities;
//using FuelMaster.HeadOffice.Core.Helpers;
//using FuelMaster.HeadOffice.Core.Models.Requests.Pumps;
//using FuelMaster.HeadOffice.Core.Resources;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.EntityFrameworkCore;

//namespace FuelMaster.HeadOffice.Controllers
//{
//    [Route("api/pumps")]
//    public class PumpsController : FuelMasterController
//    {
//        private readonly IPumpService _pumpService;

//        public PumpsController(IPumpService pumpService)
//        {
//            _pumpService = pumpService;
//        }

//        [HttpGet("pagination")]
//        public async Task<IActionResult> GetPagination([FromQuery , BindRequired] int page , [FromQuery] int? stationId)
//        {
//            return Ok(await _pumpService.GetPaginationAsync(page , stationId));
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Pump>>> GetAll([FromQuery] GetPumpRequest request)
//        {
//            var pumps = await _pumpService.GetAllAsync(request);

//            return Ok(pumps);
//        }

//        [HttpPost]
//        public async Task<ActionResult> Create([FromBody] PumpRequest pumpDto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var result = await _pumpService.CreateAsync(pumpDto);
//            if (!result.Succeeded) return BadRequest(result.Message);

//            return Ok(result.Entity);
//        }

//        [HttpPut("{id}")]
//        public async Task<ActionResult> Edit(int id, PumpRequest pumpDto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var result = await _pumpService.EditAsync(id, pumpDto);
//            if (!result.Succeeded) return BadRequest(result.Message);

//            return Ok(result.Entity);
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<Pump>> Details(int id)
//        {
//            var pump = await _pumpService.DetailsAsync(id);
//            if (pump == null)
//            {
//                return NotFound();
//            }

//            return Ok(pump);
//        }

//        [HttpDelete("{id}")]
//        public async Task<ActionResult> Delete(int id)
//        {
//            try 
//            {
//                var result = await _pumpService.DeleteAsync(id);
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
