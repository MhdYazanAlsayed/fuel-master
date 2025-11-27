using System;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Database;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories;

public class TankRepository : ITankRepository
{
    private readonly FuelMasterDbContext _context;
    public TankRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }

    public Tank Create(Tank entity)
    {
        _context.Tanks.Add(entity);
        return entity;
    }

    public Tank Delete(Tank entity)
    {
        _context.Tanks.Remove(entity);
        return entity;
    }

    public Task<List<Tank>> GetAllAsync(bool includeStation = false, bool includeFuelType = false, bool includeNozzles = false)
    {
        var query = _context.Tanks.AsQueryable();
        if (includeStation)
        {
            query = query.Include(t => t.Station);
        }
        if (includeFuelType)
        {
            query = query.Include(t => t.FuelType);
        }
        if (includeNozzles)
        {
            query = query.Include(t => t.Nozzles);
        }
        return query.ToListAsync();
    }

    public async Task<(List<Tank>, int)> GetPaginationAsync(int page, int pageSize, bool includeStation = false, bool includeFuelType = false, bool includeNozzles = false)
    {
        var query = _context.Tanks.AsQueryable();
        if (includeStation)
        {
            query = query.Include(t => t.Station);
        }
        if (includeFuelType)
        {
            query = query.Include(t => t.FuelType);
        }
        if (includeNozzles)
        {
            query = query.Include(t => t.Nozzles);
        }
        var count = await query.CountAsync();
        var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (data, count);
    }

    public Tank Update(Tank entity)
    {
        _context.Tanks.Update(entity);
        return entity;
    }
}
