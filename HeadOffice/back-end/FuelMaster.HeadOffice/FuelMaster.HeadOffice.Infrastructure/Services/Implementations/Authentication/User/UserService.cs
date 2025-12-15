using System.Security.Claims;
using FuelMaster.HeadOffice.Application.Constants;
using FuelMaster.HeadOffice.Application.DTOs.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.Authentication
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserManagerFactory _userManagerFactory;
        private readonly ITenants _tenants;
        private readonly ISigninService _signinService;

        public UserService(
           ITokenService tokenService ,
           IUserManagerFactory userManagerFactory ,
           ITenants tenants,
           ISigninService signinService)
        {

            _tokenService = tokenService;
            _userManagerFactory = userManagerFactory;
            _tenants = tenants ?? throw new ArgumentNullException(nameof(tenants));
            _signinService = signinService;
        }

        public async Task<LoginResult?> LoginAsync(LoginDto request)
        {   
            var tenantInfo = await ExtractTenantFromUserNameAsync(request.UserName);
            var tenantId = tenantInfo.TenantId;

            var userManager = await _userManagerFactory.CreateUserManagerAsync(tenantId);
            var user = await userManager.Users
                .Include(x => x.Employee)
                    .ThenInclude(x => x!.Role)
                    .ThenInclude(x => x!.AreasOfAccess)
                    .ThenInclude(x => x.AreaOfAccess)
                .SingleOrDefaultAsync(x => x.UserName == tenantInfo.UserName);

            if (user is null || !user.IsActive) 
                return null;

            bool result = await userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
                return null;


            var claims = new List<Claim>();
            claims.Add(new Claim(ConfigKeys.TanentId, tenantId.ToString()));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim("EmployeeId", user.Employee?.Id.ToString() ?? throw new NullReferenceException()));
            claims.Add(new Claim("Scope", user.Employee.Scope.ToString()));
            if (user.Employee.Scope == Scope.Station && user.Employee.StationId!= null)
            {
                claims.Add(new Claim("StationId", user.Employee.StationId.ToString()!));
            }
            else if (user.Employee.Scope == Scope.Area && user.Employee.AreaId != null)
            {
                claims.Add(new Claim("AreaId", user.Employee.AreaId.ToString()!));
            }
            else if(user.Employee.Scope == Scope.City && user.Employee.CityId != null)
            {
                claims.Add(new Claim("CityId", user.Employee.CityId.ToString()!));
            }

            var accessToken = _tokenService.GenerateAccessToken(claims);
            // TODO : Generate refresh token
            //var refreshToken = _tokenService.GenerateRefreshToken();

            var areaOfAccess = user.Employee!.Role!.AreasOfAccess
                   .Select(x => x.AreaOfAccess)
                   .Select(x => x!.AreaOfAccess)
                   .Select(AreaOfAccessMapper.Map)
                   .ToList();

            return new LoginResult()
            {
                UserName = user.UserName,
                FullName = user.Employee?.FullName ?? throw new NullReferenceException(),
                Email = user.Email,
                Scope = user.Employee.Scope,
                //AreaId = user.Employee.AreaId,
                //CityId = user.Employee.CityId,
                //StationId = user.Employee.StationId,
                AreasOfAccess = areaOfAccess,
                AccessToken = new TokenResult()
                {
                    Token = accessToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(1)
                },
                RefreshToken = null!
            };
        }
    
        public async Task<CurrentUserResult?> GetCurrentUserAsync ()
        {
            var user = await _signinService
                .GetCurrentUserAsync(includeEmployee: true, includeAreaOfAccess: true);
            if (user is null)
                return null;

            // Get areas of access
            var areaOfAccess = user.Employee!.Role!.AreasOfAccess
                .Select(x => x.AreaOfAccess)
                .Select(x => x!.AreaOfAccess)
                .Select(AreaOfAccessMapper.Map)
                .ToList();

            return new CurrentUserResult()
            {
                Email = user.Email,
                FullName = user.Employee!.FullName,
                Scope = user.Employee.Scope,
                UserName = user!.UserName!,
                AreasOfAccess = areaOfAccess
            };
        }

        private async Task<(Guid TenantId, string UserName)> ExtractTenantFromUserNameAsync(string userName)
        {
            var tenantName = userName.Split('@');
            if (tenantName.Length != 2)
                throw new Exception("Tenant name is not valid");

            var tenant = await _tenants.GetTenantByNameAsync(tenantName[1]);
            if (tenant is null)
                throw new Exception("Tenant not found");

            return (tenant.TenantId, tenantName[0]);
        }
    }
}
