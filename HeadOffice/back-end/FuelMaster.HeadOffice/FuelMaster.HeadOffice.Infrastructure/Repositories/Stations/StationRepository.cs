using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories.Stations;

public class StationRepository : IStationRepository
{
    private readonly FuelMasterDbContext _context;
    public StationRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }
    public Station Create(Station entity)
    {
        _context.Stations.Add(entity);
        return entity;
    }

    public Station Delete(Station entity)
    {
        _context.Stations.Remove(entity);
        return entity;
    }

    public async Task<Station?> DetailsAsync(int id, bool includeCity = false, bool includeZone = false, bool includeArea = false)
    {
        var query = _context.Stations.AsQueryable();
        if (includeCity)
        {
            query = query.Include(s => s.City);
        }
        if (includeZone)
        {
            query = query.Include(s => s.Zone);
        }
        if (includeArea)
        {
            query = query.Include(s => s.Area);
        }

        return await query.SingleOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Station>> GetAllAsync(bool includeCity = false, bool includeZone = false, bool includeArea = false)
    {
        var query = _context.Stations.AsQueryable();
        if (includeCity)
        {
            query = query.Include(s => s.City);
        }
        if (includeZone)
        {
            query = query.Include(s => s.Zone);
        }
        if (includeArea)
        {
            query = query.Include(s => s.Area);
        }
        return await query.ToListAsync();
    }

    public async Task<(List<Station>, int)> GetPaginationAsync(int currentPage, int pageSize, bool includeCity = false, bool includeZone = false, bool includeArea = false)
    {
        var query = _context.Stations.AsQueryable();
        if (includeCity)
        {
            query = query.Include(s => s.City);
        }
        if (includeZone)
        {
            query = query.Include(s => s.Zone);
        }
        if (includeArea)
        {
            query = query.Include(s => s.Area);
        }
        if (includeArea)
        {
            query = query.Include(s => s.Area);
        }
        var count = await query.CountAsync();
        var data = await query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        return (data, count);
    }

    public Station Update(Station entity)
    {
        _context.Stations.Update(entity);
        return entity;
    }
}
