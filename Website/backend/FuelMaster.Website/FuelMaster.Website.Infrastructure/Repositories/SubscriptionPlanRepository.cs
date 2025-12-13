using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Interfaces;
using FuelMaster.Website.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.Website.Infrastructure.Repositories;

public class SubscriptionPlanRepository : Repository<SubscriptionPlan>, ISubscriptionPlanRepository
{
    public SubscriptionPlanRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<SubscriptionPlan?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<IEnumerable<SubscriptionPlan>> GetActivePlansAsync()
    {
        return await _dbSet
            .Where(p => p.IsActive)
            .OrderBy(p => p.Price)
            .ToListAsync();
    }

    public async Task<SubscriptionPlan?> GetFreePlanAsync()
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.IsFree && p.IsActive);
    }
}

