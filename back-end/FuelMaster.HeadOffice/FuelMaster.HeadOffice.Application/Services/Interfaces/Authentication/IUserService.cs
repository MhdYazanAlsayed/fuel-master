using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Application.DTOs.Authentication;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication
{
    public interface IUserService : IScopedDependency
    {
        // Task<FuelMasterUser?> GetLoggedUserAsync();
        // Task<string?> GetLoggedUserIdAsync();
        // Task<IdentityResult> RegisterAsync(FuelMasterUser user, string password);
        Task<LoginResult?> LoginAsync(LoginDto request);
        // Task DeleteUserAsync(string userId);
        // Task<IdentityResult> UpdateAsync(FuelMasterUser user);
        // Task<IdentityResult> ResetAdminPasswordAsync(string userId);
    }
}
