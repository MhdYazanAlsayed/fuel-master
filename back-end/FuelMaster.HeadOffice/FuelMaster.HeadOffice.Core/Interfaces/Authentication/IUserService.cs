using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Authentication;
using FuelMaster.HeadOffice.Core.Models.Responses.Authentication;
using Microsoft.AspNetCore.Identity;

namespace FuelMaster.HeadOffice.Core.Interfaces.Authentication
{
    public interface IUserService : IScopedDependency
    {
        public UserManager<FuelMasterUser> UserManager { get; }
        Task<FuelMasterUser?> GetLoggedUserAsync();
        Task<string?> GetLoggedUserIdAsync();
        Task<IdentityResult> RegisterAsync(FuelMasterUser user, string password);
        Task<ResultDto<LoginResult>> LoginAsync(LoginRequest request);
        Task DeleteUserAsync(string userId);
        Task<IdentityResult> UpdateAsync(FuelMasterUser user);
        Task<IdentityResult> ResetAdminPasswordAsync(string userId);
    }
}
