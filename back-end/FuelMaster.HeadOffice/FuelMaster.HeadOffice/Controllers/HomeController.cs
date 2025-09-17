using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers
{
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        [HttpPost("home")]
        public IActionResult Home ()
        {
            return Ok();
        }
    }
}
