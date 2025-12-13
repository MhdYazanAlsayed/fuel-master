using FuelMaster.Website.Attributes.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace FuelMaster.Website.Attributes;

public class RequireActiveTenantAndSubscriptionAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Skip if already unauthorized
        if (context.Result != null)
        {
            return;
        }

        var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedObjectResult(new { message = "User not authenticated" });
            return;
        }

        // Check for active subscription from token claims
        var hasSubscriptionClaim = context.HttpContext.User.FindFirstValue("has_subscription");
        var subscriptionStatusClaim = context.HttpContext.User.FindFirstValue("subscription_status");
        var subscriptionEndDateClaim = context.HttpContext.User.FindFirstValue("subscription_end_date");

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

        if (!hasActiveSubscription)
        {
            context.Result = new ForbidObjectResult(new { 
                message = "Active subscription required", 
                code = "NO_ACTIVE_SUBSCRIPTION",
                requiresAction = "subscribe"
            });
            return;
        }

        // Check for active tenant from token claims
        var hasTenantClaim = context.HttpContext.User.FindFirstValue("has_tenant");
        var tenantStatusClaim = context.HttpContext.User.FindFirstValue("tenant_status");

        var hasActiveTenant = hasTenantClaim == "true" &&
            tenantStatusClaim == Core.Enums.TenantStatus.Active.ToString();

        if (!hasActiveTenant)
        {
            context.Result = new ForbidObjectResult(new { 
                message = "Active tenant required", 
                code = "NO_ACTIVE_TENANT",
                requiresAction = "create_tenant"
            });
            return;
        }
    }
}

// Helper class for Forbid with object result


