using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Services.Interfaces.Zones;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories.Zones;

public class ZonePriceRepository : IZonePriceRepository
{
    private readonly FuelMasterDbContext _context;
    public ZonePriceRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }
    
    public ZonePrice Create(ZonePrice entity)
    {
        _context.ZonePrices.Add(entity);
        return entity;
    }

    public ZonePrice Delete(ZonePrice entity)
    {
        _context.ZonePrices.Remove(entity);
        return entity;
    }

    public async Task<ZonePrice?> DetailsAsync(int id)
    {
        return await _context.ZonePrices.FindAsync(id);
    }

    public async Task<List<ZonePrice>> GetAllAsync()
    {
        return await _context.ZonePrices.ToListAsync();
    }

    public async Task<(List<ZonePrice>, int)> GetPaginationAsync(int page, int pageSize)
    {
        var count = await _context.ZonePrices.CountAsync();
        var data = await _context.ZonePrices.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (data, count);
    }

    public ZonePrice Update(ZonePrice entity)
    {
        _context.ZonePrices.Update(entity);
        return entity;
    }
}
