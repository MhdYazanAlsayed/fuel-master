using FuelMaster.Website.Core.Entities;

namespace FuelMaster.Website.Core.Interfaces;

public interface IBillingHistoryRepository : IRepository<BillingHistory>
{
    Task<IEnumerable<BillingHistory>> GetBillingHistoryBySubscriptionIdAsync(Guid subscriptionId);
    Task<IEnumerable<BillingHistory>> GetBillingHistoryByUserIdAsync(string userId);
    Task<BillingHistory?> GetByTransactionIdAsync(string transactionId);
}

