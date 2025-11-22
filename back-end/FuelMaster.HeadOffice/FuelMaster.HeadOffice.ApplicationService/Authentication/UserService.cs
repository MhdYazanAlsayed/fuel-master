using FuelMaster.HeadOffice.Core.Constants;
using FuelMaster.HeadOffice.Core.Interfaces.Authentication;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Authentication;
using FuelMaster.HeadOffice.Core.Models.Responses.Authentication;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Results = FuelMaster.HeadOffice.Core.Helpers.Results;

namespace FuelMaster.HeadOffice.ApplicationService.Authentication
{
    public class UserService : IUserService
    {
        private readonly UserManager<FuelMasterUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService ,
            IUserManagerFactory userManagerFactory)
        {

            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;

            //var tenantId = httpContextAccessor?.HttpContext?.Items["TenantId"];
            //if (tenantId is null)
            //    throw new NullReferenceException();

            _userManager = userManagerFactory.CreateUserManager();
        }

        public UserManager<FuelMasterUser> UserManager => _userManager;

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new Exception();

            await _userManager.DeleteAsync(user);
        }

        public async Task<FuelMasterUser?> GetLoggedUserAsync()
        {
            var claims = _httpContextAccessor.HttpContext?.User;
            if (claims is null) return null;

            return await _userManager.GetUserAsync(claims);
        }

        public async Task<string?> GetLoggedUserIdAsync()
        {
            var claims = _httpContextAccessor.HttpContext?.User;
            if (claims is null) return null;

            return (await _userManager.GetUserAsync(claims))?.Id;
        }

        public async Task<ResultDto<LoginResult>> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.Users
                .Include(x => x.Employee)
                .Include(x => x.Group)
                .Include(x => x.Group!.Permissions)
                .SingleOrDefaultAsync(x => x.UserName == request.UserName);

            if (user is null || !user.IsActive) 
                return Results.Failure<LoginResult>(Resource.InvalidLogin);


            bool result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
                return Results.Failure<LoginResult>(Resource.InvalidLogin);

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>();
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));
            claims.AddRange(new List<Claim>()
            {
                new Claim("UserName" , request.UserName),
                new Claim(ClaimTypes.NameIdentifier , user.Id),
            });

            var tenantId = _httpContextAccessor.HttpContext?.Items[ConfigKeys.TanentId]?.ToString();
            if (tenantId is null) throw new NullReferenceException();

            claims.Add(new(ConfigKeys.TanentId, tenantId));

            var accessToken = _tokenService.GenerateAccessToken(claims);
            //var refreshToken = _tokenService.GenerateRefreshToken();

            return Results.Success(new LoginResult()
            {
                AccessToken = accessToken,
                RefreshToken = null,
                Data = new UserResult()
                {
                    UserName = user.UserName,
                    FullName = user.Employee?.FullName,
                    StationId = user.Employee?.StationId,
                    Permissions = user.Group?.Permissions?.Select(x => new PermissionResult()
                    {
                        Key = x.Key ,
                        Value = x.Value
                    })
                }
            });
        }

        

        public async Task<IdentityResult> RegisterAsync(FuelMasterUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateAsync(FuelMasterUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ResetAdminPasswordAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new Exception("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return await _userManager.ResetPasswordAsync(user, token, "MyP@ssw0rd");
        }

    }
}
