using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Interfaces;
using FuelMaster.Website.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.Website.Infrastructure.Repositories;

public class UserSubscriptionRepository : Repository<UserSubscription>, IUserSubscriptionRepository
{
    public UserSubscriptionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<UserSubscription?> GetActiveSubscriptionByUserIdAsync(string userId)
    {
        return await _dbSet
            .Include(s => s.Plan)
            .FirstOrDefaultAsync(s => s.UserId == userId && s.Status == Core.Enums.SubscriptionStatus.Active);
    }

    public async Task<IEnumerable<UserSubscription>> GetSubscriptionsByUserIdAsync(string userId)
    {
        return await _dbSet
            .Include(s => s.Plan)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<UserSubscription?> GetByUserIdAndPlanIdAsync(string userId, Guid planId)
    {
        return await _dbSet
            .Include(s => s.Plan)
            .FirstOrDefaultAsync(s => s.UserId == userId && s.PlanId == planId);
    }
}

