using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Zones;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories.Zones;

public class ZonePriceHistoryRepository : IZonePriceHistoryRepository
{
    private readonly FuelMasterDbContext _context;
    public ZonePriceHistoryRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }

    public async Task<List<ZonePriceHistory>> GetAllAsync(int zonePriceId)
    {
        return await _context.ZonePriceHistory
        .Where(x => x.ZonePriceId == zonePriceId)
        .Include(x => x.User)
        .Include(x => x.User!.Employee)
        .Include(x => x.ZonePrice)
        .ThenInclude(x => x!.FuelType)
        .OrderByDescending(x => x.CreatedAt)
        .ToListAsync();
    }

    public async Task<(List<ZonePriceHistory>, int)> GetPaginationAsync(int zonePriceId, int currentPage, int pageSize)
    {
        var count = await _context.ZonePriceHistory.CountAsync(x => x.ZonePriceId == zonePriceId);
        var data = await _context.ZonePriceHistory
        .Where(x => x.ZonePriceId == zonePriceId)
        .Include(x => x.User)
        .Include(x => x.User!.Employee)
        .Include(x => x.ZonePrice)
        .ThenInclude(x => x!.FuelType)
        .OrderByDescending(x => x.CreatedAt)
        .Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        
        return (data, count);
    }
}
