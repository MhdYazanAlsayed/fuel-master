using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.Authentication
{
    public class SigninService : ISigninService
    {
        private readonly FuelMasterDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SigninService(
        IContextFactory<FuelMasterDbContext> contextFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _context = contextFactory.CurrentContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<FuelMasterUser?> GetCurrentUserAsync(bool includeEmployee = false, bool includeAreaOfAccess = false)
        {
            var claims = _httpContextAccessor.HttpContext?.User;
            if (claims is null) return null;

            var userId = claims.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return null;

            var query = _context.Users.AsQueryable();

            if (includeEmployee)
                query = query.Include(x => x.Employee);

            if (includeAreaOfAccess)
                query = query
                    .Include(x => x.Employee)
                    .ThenInclude(x => x.Role)
                    .ThenInclude(x => x.AreasOfAccess)
                    .ThenInclude(x => x.AreaOfAccess);

            return await query.SingleOrDefaultAsync(x => x.Id == userId);
        }

        public string? GetCurrentUserId()
        {
            var claims = _httpContextAccessor.HttpContext?.User;
            if (claims is null) return null;

            return claims.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public int? GetCurrentEmployeeId()
        {
            var claims = _httpContextAccessor.HttpContext?.User;
            if (claims is null) return null;

            var employeeId = claims.FindFirst(x => x.Type == "EmployeeId")?.Value;
            if (employeeId is null) return null;

            return int.Parse(employeeId);
        }
        
        public int? GetCurrentStationId()
        {
            var claims = _httpContextAccessor.HttpContext?.User;
            if (claims is null) return null;

            var stationId = claims.FindFirst(x => x.Type == "StationId")?.Value;
            if (stationId is null) return null;

            return int.Parse(stationId);
        }

        public int? GetCurrentCityId()
        {
            var claims = _httpContextAccessor.HttpContext?.User;
            if (claims is null) return null;

            var cityId = claims.FindFirst(x => x.Type == "CityId")?.Value;
            if (cityId is null) return null;

            return int.Parse(cityId);
        }

        public int? GetCurrentAreaId()
        {
            var claims = _httpContextAccessor.HttpContext?.User;
            if (claims is null) return null;

            var areaId = claims.FindFirst(x => x.Type == "AreaId")?.Value;
            if (areaId is null) return null;

            return int.Parse(areaId);
        }

        public Scope? GetCurrentScope()
        {
            var claims = _httpContextAccessor.HttpContext?.User;
            if (claims is null) return null;

            var scope = claims.FindFirst(x => x.Type == "Scope")?.Value;
            if (scope is null) return null;

            return Enum.Parse<Scope>(scope);
        }
    }
}
