using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Interfaces;
using FuelMaster.Website.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.Website.Infrastructure.Repositories;

public class BillingHistoryRepository : Repository<BillingHistory>, IBillingHistoryRepository
{
    public BillingHistoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BillingHistory>> GetBillingHistoryBySubscriptionIdAsync(Guid subscriptionId)
    {
        return await _dbSet
            .Include(b => b.UserSubscription)
                .ThenInclude(s => s.Plan)
            .Where(b => b.UserSubscriptionId == subscriptionId)
            .OrderByDescending(b => b.PaymentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<BillingHistory>> GetBillingHistoryByUserIdAsync(string userId)
    {
        return await _dbSet
            .Include(b => b.UserSubscription)
                .ThenInclude(s => s.User)
            .Include(b => b.UserSubscription)
                .ThenInclude(s => s.Plan)
            .Where(b => b.UserSubscription.UserId == userId)
            .OrderByDescending(b => b.PaymentDate)
            .ToListAsync();
    }

    public async Task<BillingHistory?> GetByTransactionIdAsync(string transactionId)
    {
        return await _dbSet
            .Include(b => b.UserSubscription)
            .FirstOrDefaultAsync(b => b.TransactionId == transactionId);
    }
}

