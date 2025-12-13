using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories.Zones;

public class ZoneRepository : IZoneRepository
{
    private readonly FuelMasterDbContext _context;
    public ZoneRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }
    public Zone Create(Zone entity)
    {
        _context.Zones.Add(entity);
        return entity;
    }

    public Zone Delete(Zone entity)
    {
        _context.Zones.Remove(entity);
        return entity;
    }

    public async Task<Zone?> DetailsAsync(int id, bool includePrices = false, bool includeFuelType = false, bool includeStations = false, bool includeTanks = false, bool includeNozzles = false)
    {
        var query = _context.Zones.AsQueryable();
        if (includePrices)
        {
            query = query.Include(z => z.Prices);
        }
        if (includeFuelType)
        {
            query = query.Include(z => z.Prices.Select(p => p.FuelType));
        }
        if (includeStations)
        {
            query = query.Include(z => z.Stations);
        }
        if (includeTanks)
        {
            query = query.Include(z => z.Stations.Select(s => s.Tanks));
        }
        if (includeNozzles)
        {
            query = query.Include(z => z.Stations.Select(s => s.Tanks.Select(t => t.Nozzles)));
        }
        return await query.SingleOrDefaultAsync(z => z.Id == id);
    }

    public async Task<List<Zone>> GetAllAsync(bool includePrices = false, bool includeFuelType = false, bool includeStations = false, bool includeTanks = false, bool includeNozzles = false)
    {
        var query = _context.Zones.AsQueryable();
        if (includePrices)
        {
            query = query.Include(z => z.Prices);
        }
        if (includeFuelType)
        {
            query = query.Include(z => z.Prices.Select(p => p.FuelType));
        }
        if (includeStations)
        {
            query = query.Include(z => z.Stations);
        }
        if (includeTanks)
        {
            query = query
            .Include(z => z.Stations)
            .ThenInclude(s => s.Tanks);
        }
        if (includeNozzles)
        {
            query = query
            .Include(z => z.Stations)
            .ThenInclude(s => s.Tanks)
            .ThenInclude(t => t.Nozzles);
        }

        return await query.ToListAsync();
    }

    public async Task<(List<Zone>, int)> GetPaginationAsync(int page, int pageSize, bool includePrices = false, bool includeFuelType = false, bool includeStations = false, bool includeTanks = false, bool includeNozzles = false)
    {
        var query = _context.Zones.AsQueryable();
        if (includePrices)
        {
            query = query.Include(z => z.Prices);
        }
        if (includeFuelType)
        {
            query = query.Include(z => z.Prices.Select(p => p.FuelType));
        }
        if (includeStations)
        {
            query = query.Include(z => z.Stations);
        }
        if (includeTanks)
        {
            query = query.Include(z => z.Stations.Select(s => s.Tanks));
        }
        if (includeNozzles)
        {
            query = query.Include(z => z.Stations.Select(s => s.Tanks.Select(t => t.Nozzles)));
        }
        var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        var count = await query.CountAsync();
        return (data, count);
    }

    public Zone Update(Zone entity)
    {
        _context.Zones.Update(entity);
        return entity;
    }
}
