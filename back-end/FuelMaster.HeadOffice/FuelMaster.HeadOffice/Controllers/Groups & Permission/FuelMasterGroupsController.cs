//using FuelMaster.HeadOffice.Core.Contracts.Entities;
//using FuelMaster.HeadOffice.Core.Entities;
//using FuelMaster.HeadOffice.Core.Helpers;
//using FuelMaster.HeadOffice.Core.Models.Dtos;
//using FuelMaster.HeadOffice.Core.Models.Requests.FuelMasterGroups;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ModelBinding;

//namespace FuelMaster.HeadOffice.Controllers
//{
//    [ApiController]
//    [Route("api/groups")]
//    public class FuelMasterGroupsController : FuelMasterController
//    {
//        private readonly IFuelMasterGroupService _fuelMasterGroupService;

//        public FuelMasterGroupsController(IFuelMasterGroupService fuelMasterGroupService)
//        {
//            _fuelMasterGroupService = fuelMasterGroupService;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<FuelMasterGroup>>> GetAll()
//        {
//            var fuelMasterGroups = await _fuelMasterGroupService.GetAllAsync();
//            return Ok(fuelMasterGroups);
//        }

//        [HttpGet("pagination")]
//        public async Task<ActionResult<PaginationDto<FuelMasterGroup>>> GetPagination([FromQuery , BindRequired] int currentPage)
//        {
//            var result = await _fuelMasterGroupService.GetPaginationAsync(currentPage);
//            return Ok(result);
//        }

//        [HttpPost]
//        public async Task<ActionResult> Create([FromBody] FuelMasterGroupDto dto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var result = await _fuelMasterGroupService.CreateAsync(dto);
//            if (!result.Succeeded) return BadRequest(result.Message);

//            return Ok(result.Entity);
//        }

//        [HttpPut("{id}")]
//        public async Task<ActionResult> Edit(int id, [FromBody] FuelMasterGroupDto dto)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

//            var result = await _fuelMasterGroupService.EditAsync(id, dto);
//            if (!result.Succeeded) return BadRequest(result.Message);

//            return Ok(result.Entity);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> Details (int id)
//        {
//            var entity = await _fuelMasterGroupService.DetailsAsync(id);
//            if (entity is null) return NotFound();

//            return Ok(entity);
//        } 

//        [HttpDelete("{id}")]
//        public async Task<ActionResult> Delete(int id)
//        {
//            var result = await _fuelMasterGroupService.DeleteAsync(id);
//            if (!result.Succeeded) return BadRequest(result.Message);

//            return Ok();
//        }
//    }

//}
