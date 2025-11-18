//using FuelMaster.HeadOffice.Core.Contracts.Database;
//using FuelMaster.HeadOffice.Core.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Serilog;

//namespace FuelMaster.HeadOffice.Controllers
//{
//    [ApiController]
//    [Route("test")]
//    public class TestController : ControllerBase
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public TestController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index ()
//        {
//            Log.Information("Test endpoint accessed");
            
//            try
//            {
//                var station = await _unitOfWork.Stations
//                    .Include(nameof(Station.City))
//                    .Include(nameof(Station.Zone))
//                    .ListAsync();

//                Log.Information("Successfully retrieved {Count} stations", station.Count);
//                return Ok(station);
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex, "Error occurred while retrieving stations");
//                return StatusCode(500, "Internal server error");
//            }
//        }
//    }
//}
