using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Interfaces;
using FuelMaster.Website.DTOs.Requests;
using FuelMaster.Website.DTOs.Responses;
using FuelMaster.Website.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FuelMaster.Website.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AuthController> _logger;
    private readonly ITokenService _tokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AuthController> logger,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        _logger.LogInformation("User {Email} registered successfully", request.Email);

        var authResponse = await GenerateAuthResponse(user);
        SetAuthCookie(authResponse.Token, authResponse.ExpiresAt);

        // Get user status from token claims
        var statusResponse = await GetUserStatusFromUserAsync(user, authResponse.Token);
        statusResponse.User = authResponse.User;
        statusResponse.ExpiresAt = authResponse.ExpiresAt;

        return Ok(statusResponse);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }

        _logger.LogInformation("User {Email} logged in successfully", request.Email);

        var authResponse = await GenerateAuthResponse(user);
        SetAuthCookie(authResponse.Token, authResponse.ExpiresAt);

        // Get user status from token claims
        var statusResponse = await GetUserStatusFromUserAsync(user, authResponse.Token);
        statusResponse.User = authResponse.User;
        statusResponse.ExpiresAt = authResponse.ExpiresAt;

        return Ok(statusResponse);
    }

    [HttpPost("google")]
    public IActionResult GoogleLogin()
    {
        var redirectUrl = Url.Action(nameof(GoogleCallback), "Auth");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        return Challenge(properties, "Google");
    }

    [HttpGet("google-callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return BadRequest(new { message = "Error loading external login information" });
        }

        var signInResult = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

        if (signInResult.Succeeded)
        {
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user != null)
            {
                var authResponse = await GenerateAuthResponse(user);
                SetAuthCookie(authResponse.Token, authResponse.ExpiresAt);
                return Ok(new AuthResponse
                {
                    ExpiresAt = authResponse.ExpiresAt,
                    User = authResponse.User
                });
            }
        }

        // User doesn't exist, create new account
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;
        var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;

        if (email == null)
        {
            return BadRequest(new { message = "Email not provided by Google" });
        }

        var newUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            CreatedAt = DateTime.UtcNow
        };

        var createResult = await _userManager.CreateAsync(newUser);
        if (!createResult.Succeeded)
        {
            return BadRequest(new { message = "Failed to create user account" });
        }

        await _userManager.AddLoginAsync(newUser, info);
        var newAuthResponse = await GenerateAuthResponse(newUser);
        SetAuthCookie(newAuthResponse.Token, newAuthResponse.ExpiresAt);
        return Ok(new AuthResponse
        {
            ExpiresAt = newAuthResponse.ExpiresAt,
            User = newAuthResponse.User
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.DeleteAuthCookie();
        return Ok(new { message = "Logged out successfully" });
    }

    [Authorize]
    [HttpGet("check")]
    public IActionResult Check()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var response = GetUserStatusFromClaims(User);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking user status");
            return StatusCode(500, new { message = "An error occurred while checking user status" });
        }
    }

    private async Task<(string Token, DateTime ExpiresAt, UserInfo User)> GenerateAuthResponse(ApplicationUser user)
    {
        var (token, expiresAt) = await _tokenService.GenerateTokenAsync(user);

        return (token, expiresAt, new UserInfo
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName
        });
    }

    private void SetAuthCookie(string token, DateTime expiresAt)
    {
        Response.SetAuthCookie(token, expiresAt);
    }

    private UserStatusResponse GetUserStatusFromClaims(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        var fullName = claimsPrincipal.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        var nameParts = fullName.Split(' ', 2);
        var firstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
        var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;

        // Check for active subscription from token claims
        var hasSubscriptionClaim = claimsPrincipal.FindFirstValue("has_subscription");
        var subscriptionStatusClaim = claimsPrincipal.FindFirstValue("subscription_status");
        var subscriptionEndDateClaim = claimsPrincipal.FindFirstValue("subscription_end_date");

        var hasActiveSubscription = hasSubscriptionClaim == "true" &&
            subscriptionStatusClaim == Core.Enums.SubscriptionStatus.Active.ToString();

        // If subscription has an end date, check if it hasn't expired
        if (hasActiveSubscription && !string.IsNullOrEmpty(subscriptionEndDateClaim))
        {
            if (DateTime.TryParse(subscriptionEndDateClaim, out var endDate))
            {
                if (endDate < DateTime.UtcNow)
                {
                    hasActiveSubscription = false;
                }
            }
        }

        // Check for active tenant from token claims
        var hasTenantClaim = claimsPrincipal.FindFirstValue("has_tenant");
        var tenantStatusClaim = claimsPrincipal.FindFirstValue("tenant_status");

        var hasActiveTenant = hasTenantClaim == "true" &&
            tenantStatusClaim == Core.Enums.TenantStatus.Active.ToString();

        // User can perform operations only if they have both active subscription and active tenant
        var canPerformOperations = hasActiveSubscription && hasActiveTenant;

        return new UserStatusResponse
        {
            IsAuthenticated = true,
            HasActiveSubscription = hasActiveSubscription,
            HasActiveTenant = hasActiveTenant,
            CanPerformOperations = canPerformOperations,
            User = new UserInfo
            {
                Id = userId,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            },
            Message = canPerformOperations
                ? null
                : !hasActiveSubscription
                    ? "Please subscribe to a plan to continue"
                    : "Please create a tenant to continue"
        };
    }

    private async Task<UserStatusResponse> GetUserStatusFromUserAsync(ApplicationUser user, string? existingToken = null)
    {
        if (existingToken is not null)
        {
            var handler1 = new JwtSecurityTokenHandler();
            var jsonToken1 = handler1.ReadJwtToken(existingToken);
            var claimsPrincipal1 = new ClaimsPrincipal(new ClaimsIdentity(jsonToken1.Claims));
            return GetUserStatusFromClaims(claimsPrincipal1);
        }

        // Generate token to get claims, then extract status
        var (token, _) = await _tokenService.GenerateTokenAsync(user);
        
        // Decode token to get claims
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(jsonToken.Claims));

        return GetUserStatusFromClaims(claimsPrincipal);
    }
}

