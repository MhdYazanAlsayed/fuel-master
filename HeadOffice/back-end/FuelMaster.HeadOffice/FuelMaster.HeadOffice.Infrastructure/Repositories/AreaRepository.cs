using FuelMaster.HeadOffice.Core.Entities.Configs.Area;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories;

public class AreaRepository : IAreaRepository
{
    private readonly FuelMasterDbContext _context;

    public AreaRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }

    public Area Create(Area entity)
    {
        _context.Areas.Add(entity);
        return entity;
    }

    public Area Delete(Area entity)
    {
        _context.Areas.Remove(entity);
        return entity;
    }

    public async Task<Area?> DetailsAsync(int id, bool includeStations = false)
    {
        var query = _context.Areas.AsQueryable();

        if (includeStations)
        {
            query = query.Include(a => a.Stations);
        }

        return await query.SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Area>> GetAllAsync(bool includeStations = false)
    {
        var query = _context.Areas.AsQueryable();

        if (includeStations)
        {
            query = query.Include(a => a.Stations);
        }

        return await query.ToListAsync();
    }

    public async Task<(List<Area>, int)> GetPaginationAsync(int page, int pageSize, bool includeStations = false)
    {
        var query = _context.Areas.AsQueryable();

        if (includeStations)
        {
            query = query.Include(a => a.Stations);
        }

        var count = await query.CountAsync();
        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, count);
    }

    public Area Update(Area entity)
    {
        _context.Areas.Update(entity);
        return entity;
    }
}

