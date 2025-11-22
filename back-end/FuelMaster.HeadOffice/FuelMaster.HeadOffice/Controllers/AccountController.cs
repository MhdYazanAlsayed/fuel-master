using FuelMaster.HeadOffice.Core.Interfaces.Authentication;
using FuelMaster.HeadOffice.Core.Interfaces.Entities;
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

            try 
            {
                var result = await _userService.LoginAsync(request);
                if (!result.Succeeded) return BadRequest(result.Entity);

                return Ok(result.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update-password") , Authorize]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] EditCurrentPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest();

            try 
            {
                
                var result = await _accountService.EditCurrentPassword(dto);
                if (!result.Succeeded) return BadRequest(result.Message);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
