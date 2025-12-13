using FuelMaster.Website.Core.Entities;

namespace FuelMaster.Website.Core.Interfaces;

public interface ISubscriptionService
{
    Task<IEnumerable<SubscriptionPlan>> GetAllPlansAsync();
    Task<IEnumerable<SubscriptionPlan>> GetActivePlansAsync();
    Task<SubscriptionPlan?> GetPlanByIdAsync(Guid planId);
    Task<UserSubscription> SubscribeUserAsync(string userId, Guid planId);
    Task<UserSubscription?> GetUserActiveSubscriptionAsync(string userId);
    Task<IEnumerable<UserSubscription>> GetUserSubscriptionsAsync(string userId);
    Task<bool> CancelSubscriptionAsync(string userId, Guid subscriptionId);
}

