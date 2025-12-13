using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Interfaces;
using FuelMaster.Website.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.Website.Infrastructure.Repositories;

public class TenantRepository : Repository<Tenant>, ITenantRepository
{
    public TenantRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Tenant?> GetByNameAsync(string name)
    {
        return await _dbSet
            .Include(t => t.Plan)
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<Tenant?> GetByUserIdAsync(string userId)
    {
        return await _dbSet
            .Include(t => t.Plan)
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.UserId == userId);
    }

    public async Task<IEnumerable<Tenant>> GetTenantsByUserIdAsync(string userId)
    {
        return await _dbSet
            .Include(t => t.Plan)
            .Include(t => t.User)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> TenantNameExistsAsync(string name)
    {
        return await _dbSet.AnyAsync(t => t.Name == name);
    }
}

