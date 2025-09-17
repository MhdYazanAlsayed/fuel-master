using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Models.Requests.Accounts;
using FuelMaster.HeadOffice.Core.Models.Requests.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers
{
    [Route("api/account")]
    public class AccountController(IUserService _userService ,
        IAccountService _accountService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync ([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _userService.LoginAsync(request);
            if (!result.Succeeded) return BadRequest(result.Entity);

            return Ok(result.Entity);
        }

        [HttpPost("update-password") , Authorize]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] EditCurrentPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _accountService.EditCurrentPassword(dto);
            if (!result.Succeeded) return BadRequest(result.Message);

            return Ok();
        }

        [HttpGet("reset")]
        public async Task<IActionResult> ResetAsync ()
        {
            await _accountService.Reset();

            return Ok();
        }
    }
}
