using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories;

public class PumpRepository : IPumpRepository
{
    private readonly FuelMasterDbContext _context;
    
    public PumpRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }

    public Pump Create(Pump entity)
    {
        _context.Pumps.Add(entity);
        return entity;
    }

    public Pump Delete(Pump entity)
    {
        _context.Pumps.Remove(entity);
        return entity;
    }

    public async Task<Pump?> DetailsAsync(int id, bool includeStation = false, bool includeNozzles = false)
    {
        var query = _context.Pumps.AsQueryable();

        if (includeStation)
        {
            query = query.Include(x => x.Station); 
        }

        if (includeNozzles)
        {
            query = query.Include(x => x.Nozzles);
        }

        return await query.SingleOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<Pump>> GetAllAsync(bool includeStation = false, bool includeNozzles = false)
    {
        var query = _context.Pumps.AsQueryable();
        
        if (includeStation)
        {
            query = query.Include(p => p.Station);
        }
        
        if (includeNozzles)
        {
            query = query.Include(p => p.Nozzles);
        }
        
        return query.ToListAsync();
    }

    public async Task<(List<Pump>, int)> GetPaginationAsync(int page, int pageSize, bool includeStation = false, bool includeNozzles = false)
    {
        var query = _context.Pumps.AsQueryable();
        
        if (includeStation)
        {
            query = query.Include(p => p.Station);
        }
        
        if (includeNozzles)
        {
            query = query.Include(p => p.Nozzles);
        }
        
        var count = await query.CountAsync();
        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (data, count);
    }

    public Pump Update(Pump entity)
    {
        _context.Pumps.Update(entity);
        return entity;
    }
}

