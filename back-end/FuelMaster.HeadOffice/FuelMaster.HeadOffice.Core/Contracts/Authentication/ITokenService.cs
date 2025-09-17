using FuelMaster.HeadOffice.Core.Contracts.Markers;
using System.Security.Claims;

namespace FuelMaster.HeadOffice.Core.Contracts.Authentication
{
    public interface ITokenService : IScopedDependency
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
