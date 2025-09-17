using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FuelMaster.HeadOffice.ApplicationService.Authentication
{
    public class AuthorizationService : IAuthorization
    {
        private readonly UserManager<FuelMasterUser> _userManager;
        private readonly FuelMasterDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationService(
        UserManager<FuelMasterUser> userManager , 
        IContextFactory<FuelMasterDbContext> contextFactory,
        IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = contextFactory.CurrentContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<FuelMasterUser?> GetLoggedUserAsync(bool includeEmployee = false)
        {
            var claims = _httpContextAccessor.HttpContext?.User;
            if (claims is null) return null;

            var userId = claims.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return null;


            if (includeEmployee)
                return await _context.Users
                            .Include(x => x.Employee)
                            .SingleOrDefaultAsync(x => x.Id == userId);

            return await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<string?> GetLoggedUserIdAsync()
        {
            var claims = _httpContextAccessor.HttpContext?.User;
            if (claims is null) return null;

            return (await _userManager.GetUserAsync(claims))?.Id;
        }

        public async Task<int?> TryToGetStationIdAsync()
        {
            int? stationId = null;
            var user = await GetLoggedUserAsync(true);
            if (user is null) 
                throw new Exception();
            
            if (user.Employee?.StationId is not null)
            {
                stationId = user.Employee.StationId;
            }

            return stationId;
        }
    }
}
