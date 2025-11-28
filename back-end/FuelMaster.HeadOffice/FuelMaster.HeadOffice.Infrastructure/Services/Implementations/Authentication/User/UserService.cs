// using FuelMaster.HeadOffice.Core.Interfaces.Authentication;
// using FuelMaster.HeadOffice.Core.Entities;
// using FuelMaster.HeadOffice.Core.Models.Requests.Authentication;
// using FuelMaster.HeadOffice.Core.Models.Responses.Authentication;
// using FuelMaster.HeadOffice.Core.Resources;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using System.Security.Claims;

using System.Security.Claims;
using FuelMaster.HeadOffice.Application.DTOs.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;
using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.Authentication
{
    public class UserService : IUserService
    {
        private readonly UserManager<FuelMasterUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ITanentService _tanentService;
        public UserService(
           ITokenService tokenService ,
           IUserManagerFactory userManagerFactory,
           ITanentService tanentService)
        {

           _tokenService = tokenService;
           _userManager = userManagerFactory.CreateUserManager();
           _tanentService = tanentService ?? throw new ArgumentNullException(nameof(tanentService));
        }

        public async Task<LoginResult?> LoginAsync(LoginDto request)
        {
            var user = await _userManager.Users
                .Include(x => x.Employee)
                .SingleOrDefaultAsync(x => x.UserName == request.UserName);

            if (user is null || !user.IsActive) 
                return null;

            bool result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
                return null;

            var tenantId = _tanentService.TenantId;
            if (tenantId is null) throw new NullReferenceException();

            var claims = new List<Claim>();
            claims.Add(new Claim("TenantId", tenantId));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName!.ToString()));
            claims.Add(new Claim("EmployeeId", user.Employee?.Id.ToString() ?? throw new NullReferenceException()));


            var accessToken = _tokenService.GenerateAccessToken(claims);
            // TODO : Generate refresh token
            //var refreshToken = _tokenService.GenerateRefreshToken();

            return new LoginResult()
            {
                UserName = user.UserName,
                FullName = user.Employee?.FullName ?? throw new NullReferenceException(),
                Email = user.Email,
                AccessToken = new TokenResult()
                {
                    Token = accessToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                },
                RefreshToken = null!
            };
        }
    }
}
