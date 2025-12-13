using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using System.Security.Claims;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication
{
    public interface ITokenService : IScopedDependency
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
