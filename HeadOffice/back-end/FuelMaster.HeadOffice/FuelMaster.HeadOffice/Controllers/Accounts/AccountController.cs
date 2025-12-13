using FuelMaster.HeadOffice.Application.DTOs.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuelMaster.HeadOffice.Controllers.Accounts
{
    [Route("api/auth")]
    public class AccountController(IUserService _userService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync ([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid) return BadRequest();

            try 
            {
                var result = await _userService.LoginAsync(request);
                if (result is null) return BadRequest("Invalid username or password");

                // Set cookies for access token and refresh token
                Response.Cookies.Append("access_token", result.AccessToken.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = result.AccessToken.ExpiresAt
                });
                //Response.Cookies.Append("refresh_token", result.RefreshToken.Token, new CookieOptions
                //{
                //    HttpOnly = true,
                //    Expires = result.RefreshToken.ExpiresAt
                //});

                return Ok(new
                {
                    UserName = result.UserName,
                    FullName = result.FullName,
                    Email = result.Email,
                    Scope = result.Scope,
                    CityId = result.CityId,
                    StationId = result.StationId,
                    AreaId = result.AreaId,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("logout")]
        public IActionResult Logout ()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1), // Set to past date to ensure deletion
                Path = "/"
            };
            Response.Cookies.Append("access_token", string.Empty, cookieOptions);
            Response.Cookies.Append("refresh_token", string.Empty, cookieOptions);

            return Ok();
        }

        [Authorize]
        [HttpGet("health")]
        public IActionResult health()
        {
            return Ok("You are logged in");
        }
    }
}
