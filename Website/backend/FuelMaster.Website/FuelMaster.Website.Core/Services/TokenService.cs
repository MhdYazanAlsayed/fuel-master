using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FuelMaster.Website.Core.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITenantService _tenantService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IConfiguration _configuration;

    public TokenService(
        UserManager<ApplicationUser> userManager,
        ITenantService tenantService,
        ISubscriptionService subscriptionService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _tenantService = tenantService;
        _subscriptionService = subscriptionService;
        _configuration = configuration;
    }

    public async Task<(string Token, DateTime ExpiresAt)> GenerateTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Check for tenant
        var tenant = await _tenantService.GetTenantByUserIdAsync(user.Id);
        if (tenant != null)
        {
            claims.Add(new Claim("has_tenant", "true"));
            claims.Add(new Claim("tenant_status", tenant.Status.ToString()));
        }
        else
        {
            claims.Add(new Claim("has_tenant", "false"));
        }

        // Check for active subscription
        var activeSubscription = await _subscriptionService.GetUserActiveSubscriptionAsync(user.Id);
        if (activeSubscription != null)
        {
            claims.Add(new Claim("has_subscription", "true"));
            claims.Add(new Claim("subscription_status", activeSubscription.Status.ToString()));
            if (activeSubscription.EndDate.HasValue)
            {
                claims.Add(new Claim("subscription_end_date", activeSubscription.EndDate.Value.ToString("O"))); // ISO 8601 format
            }
        }
        else
        {
            claims.Add(new Claim("has_subscription", "false"));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddHours(Convert.ToDouble(
            _configuration["Jwt:ExpireHours"] ?? "24"));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenString, expires);
    }
}

