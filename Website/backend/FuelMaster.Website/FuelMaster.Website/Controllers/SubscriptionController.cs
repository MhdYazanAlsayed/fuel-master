using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Interfaces;
using FuelMaster.Website.DTOs.Requests;
using FuelMaster.Website.DTOs.Responses;
using FuelMaster.Website.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FuelMaster.Website.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ILogger<SubscriptionController> _logger;
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public SubscriptionController(
        ISubscriptionService subscriptionService,
        ILogger<SubscriptionController> logger,
        ITokenService tokenService,
        UserManager<ApplicationUser> userManager)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    [HttpGet("plans")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPlans([FromQuery] bool activeOnly = true)
    {
        try
        {
            var plans = activeOnly
                ? await _subscriptionService.GetActivePlansAsync()
                : await _subscriptionService.GetAllPlansAsync();

            var response = plans.Select(p => new SubscriptionPlanResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                BillingCycle = p.BillingCycle.ToString(),
                IsFree = p.IsFree,
                Features = p.Features,
                IsActive = p.IsActive
            });

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving subscription plans");
            return StatusCode(500, new { message = "An error occurred while retrieving subscription plans" });
        }
    }

    [HttpGet("plans/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPlanById(Guid id)
    {
        try
        {
            var plan = await _subscriptionService.GetPlanByIdAsync(id);
            if (plan == null)
            {
                return NotFound(new { message = "Subscription plan not found" });
            }

            var response = new SubscriptionPlanResponse
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                BillingCycle = plan.BillingCycle.ToString(),
                IsFree = plan.IsFree,
                Features = plan.Features,
                IsActive = plan.IsActive
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving subscription plan {PlanId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the subscription plan" });
        }
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var subscription = await _subscriptionService.SubscribeUserAsync(userId, request.PlanId);

            // Refresh token to include updated subscription information
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var (token, expiresAt) = await _tokenService.GenerateTokenAsync(user);
                Response.SetAuthCookie(token, expiresAt);
            }

            var plan = await _subscriptionService.GetPlanByIdAsync(request.PlanId);
            var response = new UserSubscriptionResponse
            {
                Id = subscription.Id,
                PlanId = subscription.PlanId,
                Plan = new SubscriptionPlanResponse
                {
                    Id = plan!.Id,
                    Name = plan.Name,
                    Description = plan.Description,
                    Price = plan.Price,
                    BillingCycle = plan.BillingCycle.ToString(),
                    IsFree = plan.IsFree,
                    Features = plan.Features,
                    IsActive = plan.IsActive
                },
                Status = subscription.Status.ToString(),
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                NextBillingDate = subscription.NextBillingDate,
                CreatedAt = subscription.CreatedAt
            };

            _logger.LogInformation("User {UserId} subscribed to plan {PlanId}", userId, request.PlanId);

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error subscribing user to plan {PlanId}", request.PlanId);
            return StatusCode(500, new { message = "An error occurred while processing the subscription" });
        }
    }

    [HttpGet("my-subscription")]
    public async Task<IActionResult> GetMyActiveSubscription()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var subscription = await _subscriptionService.GetUserActiveSubscriptionAsync(userId);
            if (subscription == null)
            {
                return NotFound(new { message = "No active subscription found" });
            }

            var response = new UserSubscriptionResponse
            {
                Id = subscription.Id,
                PlanId = subscription.PlanId,
                Plan = new SubscriptionPlanResponse
                {
                    Id = subscription.Plan.Id,
                    Name = subscription.Plan.Name,
                    Description = subscription.Plan.Description,
                    Price = subscription.Plan.Price,
                    BillingCycle = subscription.Plan.BillingCycle.ToString(),
                    IsFree = subscription.Plan.IsFree,
                    Features = subscription.Plan.Features,
                    IsActive = subscription.Plan.IsActive
                },
                Status = subscription.Status.ToString(),
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                NextBillingDate = subscription.NextBillingDate,
                CreatedAt = subscription.CreatedAt
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user subscription");
            return StatusCode(500, new { message = "An error occurred while retrieving the subscription" });
        }
    }

    [HttpGet("my-subscriptions")]
    public async Task<IActionResult> GetMySubscriptions()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var subscriptions = await _subscriptionService.GetUserSubscriptionsAsync(userId);

            var response = subscriptions.Select(s => new UserSubscriptionResponse
            {
                Id = s.Id,
                PlanId = s.PlanId,
                Plan = new SubscriptionPlanResponse
                {
                    Id = s.Plan.Id,
                    Name = s.Plan.Name,
                    Description = s.Plan.Description,
                    Price = s.Plan.Price,
                    BillingCycle = s.Plan.BillingCycle.ToString(),
                    IsFree = s.Plan.IsFree,
                    Features = s.Plan.Features,
                    IsActive = s.Plan.IsActive
                },
                Status = s.Status.ToString(),
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                NextBillingDate = s.NextBillingDate,
                CreatedAt = s.CreatedAt
            });

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user subscriptions");
            return StatusCode(500, new { message = "An error occurred while retrieving subscriptions" });
        }
    }

    [HttpPost("{subscriptionId}/cancel")]
    public async Task<IActionResult> CancelSubscription(Guid subscriptionId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var result = await _subscriptionService.CancelSubscriptionAsync(userId, subscriptionId);
            if (!result)
            {
                return NotFound(new { message = "Subscription not found or does not belong to user" });
            }

            _logger.LogInformation("User {UserId} cancelled subscription {SubscriptionId}", userId, subscriptionId);

            return Ok(new { message = "Subscription cancelled successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling subscription {SubscriptionId}", subscriptionId);
            return StatusCode(500, new { message = "An error occurred while cancelling the subscription" });
        }
    }
}

