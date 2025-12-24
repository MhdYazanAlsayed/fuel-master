using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories;

public class NozzleRepository : INozzleRepository
{
    private readonly FuelMasterDbContext _context;
    
    public NozzleRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }

    public Nozzle Create(Nozzle entity)
    {
        _context.Nozzles.Add(entity);
        return entity;
    }

    public Nozzle Delete(Nozzle entity)
    {
        _context.Nozzles.Remove(entity);
        return entity;
    }

    public async Task<Nozzle?> DetailsAsync(int id, bool includeStation = false, bool includeTank = false, bool includePump = false, bool includeFuelType = false)
    {
        var query = _context.Nozzles.AsQueryable();
        
        if (includeStation)
        {
            query = query
                .Include(x => x.Tank)
                .ThenInclude(x => x.Station);
        }

        // If includeStation = true -> Tank is already included
        if (!includeStation && includeTank)
        {
            query = query.Include(x => x.Tank);
        }

        if (includePump)
        {
            query = query.Include(x => x.Pump);
        }

        if (includeFuelType)
        {
            query = query.Include(x => x.FuelType);
        }

        return await query.SingleOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<Nozzle>> GetAllAsync(bool includeStation = false, bool includeTank = false, bool includePump = false, bool includeFuelType = false)
    {
        var query = _context.Nozzles.AsQueryable();

        if (includeStation)
        {
            query = query
                .Include(x => x.Tank)
                .ThenInclude(x => x.Station);
        }

        // If includeStation = true -> Tank is already included
        if (!includeStation && includeTank)
        {
            query = query.Include(x => x.Tank);
        }

        if (includePump)
        {
            query = query.Include(n => n.Pump);
        }
        
        if (includeFuelType)
        {
            query = query.Include(n => n.FuelType);
        }
        
        return query.ToListAsync();
    }

    public async Task<List<Nozzle>> GetAllByStationIdAsync(int stationId, bool includeTank = false, bool includePump = false, bool includeFuelType = false)
    {
        var query = _context.Nozzles.AsQueryable();

        if (includeTank)
        {
            query = query.Include(n => n.Tank);
        }

        if (includePump)
        {
            query = query.Include(n => n.Pump);
        }

        if (includeFuelType)
        {
            query = query.Include(n => n.FuelType);
        }

        return await query.Where(x => x.Tank!.StationId == stationId).ToListAsync();
    }

    public async Task<(List<Nozzle>, int)> GetPaginationAsync(int page, int pageSize, bool includeStation = false, bool includeTank = false, bool includePump = false, bool includeFuelType = false)
    {
        var query = _context.Nozzles.AsQueryable();

        if (includeStation)
        {
            query = query
                .Include(x => x.Tank)
                .ThenInclude(x => x.Station);
        }

        // If includeStation = true -> Tank is already included
        if (!includeStation && includeTank)
        {
            query = query.Include(x => x.Tank);
        }

        if (includePump)
        {
            query = query.Include(n => n.Pump);
        }
        
        if (includeFuelType)
        {
            query = query.Include(n => n.FuelType);
        }
        
        var count = await query.CountAsync();
        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (data, count);
    }

    public Nozzle Update(Nozzle entity)
    {
        _context.Nozzles.Update(entity);
        return entity;
    }
}

