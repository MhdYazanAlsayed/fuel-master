using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Enums;
using FuelMaster.Website.Core.Interfaces;

namespace FuelMaster.Website.Core.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionPlanRepository _planRepository;
    private readonly IUserSubscriptionRepository _subscriptionRepository;

    public SubscriptionService(
        ISubscriptionPlanRepository planRepository,
        IUserSubscriptionRepository subscriptionRepository)
    {
        _planRepository = planRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<IEnumerable<SubscriptionPlan>> GetAllPlansAsync()
    {
        return await _planRepository.GetAllAsync();
    }

    public async Task<IEnumerable<SubscriptionPlan>> GetActivePlansAsync()
    {
        return await _planRepository.GetActivePlansAsync();
    }

    public async Task<SubscriptionPlan?> GetPlanByIdAsync(Guid planId)
    {
        return await _planRepository.GetByIdAsync(planId);
    }

    public async Task<UserSubscription> SubscribeUserAsync(string userId, Guid planId)
    {
        // Check if plan exists
        var plan = await _planRepository.GetByIdAsync(planId);
        if (plan == null)
        {
            throw new ArgumentException("Subscription plan not found", nameof(planId));
        }

        if (!plan.IsActive)
        {
            throw new InvalidOperationException("Subscription plan is not active");
        }

        // Check if user already has an active subscription
        var existingSubscription = await _subscriptionRepository.GetActiveSubscriptionByUserIdAsync(userId);
        if (existingSubscription != null)
        {
            // Cancel existing subscription
            existingSubscription.Status = SubscriptionStatus.Cancelled;
            existingSubscription.EndDate = DateTime.UtcNow;
            existingSubscription.UpdatedAt = DateTime.UtcNow;
            await _subscriptionRepository.UpdateAsync(existingSubscription);
        }

        // Create new subscription
        var subscription = new UserSubscription
        {
            UserId = userId,
            PlanId = planId,
            Status = SubscriptionStatus.Active,
            StartDate = DateTime.UtcNow,
            NextBillingDate = CalculateNextBillingDate(plan)
        };

        return await _subscriptionRepository.AddAsync(subscription);
    }

    public async Task<UserSubscription?> GetUserActiveSubscriptionAsync(string userId)
    {
        return await _subscriptionRepository.GetActiveSubscriptionByUserIdAsync(userId);
    }

    public async Task<IEnumerable<UserSubscription>> GetUserSubscriptionsAsync(string userId)
    {
        return await _subscriptionRepository.GetSubscriptionsByUserIdAsync(userId);
    }

    public async Task<bool> CancelSubscriptionAsync(string userId, Guid subscriptionId)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId);
        if (subscription == null || subscription.UserId != userId)
        {
            return false;
        }

        subscription.Status = SubscriptionStatus.Cancelled;
        subscription.EndDate = DateTime.UtcNow;
        subscription.UpdatedAt = DateTime.UtcNow;
        await _subscriptionRepository.UpdateAsync(subscription);

        return true;
    }

    private DateTime? CalculateNextBillingDate(SubscriptionPlan plan)
    {
        if (plan.IsFree)
        {
            return null; // Free plans don't have billing dates
        }

        return plan.BillingCycle == BillingCycle.Monthly
            ? DateTime.UtcNow.AddMonths(1)
            : DateTime.UtcNow.AddYears(1);
    }
}

