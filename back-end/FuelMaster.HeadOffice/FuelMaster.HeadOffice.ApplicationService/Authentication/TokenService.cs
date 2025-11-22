using FuelMaster.HeadOffice.Core.Configurations;
using FuelMaster.HeadOffice.Core.Interfaces.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FuelMaster.HeadOffice.ApplicationService.Authentication
{
    public class TokenService(
        AuthorizationConfiguration _authorizationSettings) : ITokenService
    {
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var keyToken = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_authorizationSettings.Key)
            );

            var _token = new JwtSecurityToken(
                issuer: _authorizationSettings.Issuer,
                audience: _authorizationSettings.Audience,
                claims: claims.ToArray(),
                expires: null,
                signingCredentials: new SigningCredentials(keyToken, SecurityAlgorithms.HmacSha256)
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(_token);

            return accessToken;
        }

        public string GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_authorizationSettings.Key);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = _authorizationSettings.ValidateIssuer,
                ValidateAudience = _authorizationSettings.ValidateAudience,
                ValidateLifetime = _authorizationSettings.RequireExpirationTime,
                RequireExpirationTime = _authorizationSettings.RequireExpirationTime,
                ValidateIssuerSigningKey = _authorizationSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
