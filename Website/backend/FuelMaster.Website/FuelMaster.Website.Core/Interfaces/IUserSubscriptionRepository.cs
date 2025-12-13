using FuelMaster.Website.Core.Entities;

namespace FuelMaster.Website.Core.Interfaces;

public interface IUserSubscriptionRepository : IRepository<UserSubscription>
{
    Task<UserSubscription?> GetActiveSubscriptionByUserIdAsync(string userId);
    Task<IEnumerable<UserSubscription>> GetSubscriptionsByUserIdAsync(string userId);
    Task<UserSubscription?> GetByUserIdAndPlanIdAsync(string userId, Guid planId);
}

