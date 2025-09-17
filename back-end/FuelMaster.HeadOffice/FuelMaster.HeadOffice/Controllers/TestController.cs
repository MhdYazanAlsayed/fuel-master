using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index ()
        {
            var station = await _unitOfWork.Stations
                .Include(nameof(Station.City))
                .Include(nameof(Station.Zone))
                .ListAsync();

            return Ok(station);
        }
    }
}
