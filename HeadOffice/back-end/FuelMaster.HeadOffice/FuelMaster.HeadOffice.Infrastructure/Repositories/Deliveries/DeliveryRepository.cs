using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Deliveries;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Repositories.Deliveries;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly FuelMasterDbContext _context;

    public DeliveryRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }

    public IDeliveryReadQuery UseScopeFilter()
    {
        return new DeliveryReadQuery(_context.Deliveries.AsQueryable());
    }

    public Delivery Create(Delivery entity)
    {
        _context.Deliveries.Add(entity);
        return entity;
    }

    public Delivery Delete(Delivery entity)
    {
        _context.Deliveries.Remove(entity);
        return entity;
    }

    public async Task<Delivery?> DetailsAsync(int id, bool includeStation = false, bool includeTank = false)
    {
        var query = _context.Deliveries.AsQueryable();

        if (includeStation)
        {
            query = query.Include(d => d.Tank)
            .ThenInclude(x => x.Station);
        }
        else if (includeTank)
        {
            query = query.Include(d => d.Tank);
        }

        return await query.SingleOrDefaultAsync(d => d.Id == id);
    }

    public async Task<(List<Delivery>, int)> GetPaginationAsync(
        int page, 
        int pageSize,
        DateTime? from = null,
        DateTime? to = null,
        int? cityId = null,
        int? areaId = null,
        int? stationId = null,
        int? tankId = null,
        bool includeStation = false, 
        bool includeTank = false)
    {
        var query = _context.Deliveries.AsQueryable();

        if (from is not null)
        {
            query = query.Where(d => d.CreatedAt >= from);
        }
        if (to is not null)
        {
            query = query.Where(d => d.CreatedAt <= to);
        }

        if (cityId is not null)
        {
            query = query
            .Include(x => x.Tank)
            .ThenInclude(x => x!.Station)
            .Where(d => d.Tank!.Station!.CityId == cityId);
        }
        if (areaId is not null)
        {
            query = query
            .Include(x => x.Tank)
            .ThenInclude(x => x!.Station)
            .Where(d => d.Tank!.Station!.AreaId == areaId);
        }
        if (stationId is not null)
        {
            query = query
            .Include(x => x.Tank)
            .Where(d => d.Tank!.StationId == stationId);
        }

        if (tankId is not null)
        {
            query = query
            .Where(d => d.TankId == tankId);
        }

        if (includeStation)
        {
            query = query.Include(d => d.Tank)
            .ThenInclude(x => x.Station);
        }
        else if (includeTank)
        {
            query = query.Include(d => d.Tank);
        }

        var count = await query.CountAsync();
        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, count);
    }

    public Delivery Update(Delivery entity)
    {
        _context.Deliveries.Update(entity);
        return entity;
    }

    public async Task<List<Delivery>> GetAllAsync(
        DateTime from, 
        DateTime to, 
        int? cityId = null, 
        int? areaId = null, 
        int? stationId = null, 
        int? tankId = null, 
        bool includeStation = false, 
        bool includeTank = false)
    {
        var query = _context.Deliveries.AsQueryable();

        query = query.Where(x => x.CreatedAt >= from && x.CreatedAt <= to);

        if (cityId is not null)
        {
            query = query
            .Include(x => x.Tank)
            .ThenInclude(x => x!.Station)
            .Where(x => x.Tank!.Station!.CityId == cityId);
        }
        if (areaId is not null)
        {
            query = query.Include(x => x.Tank)
            .ThenInclude(x => x!.Station)
            .Where(x => x.Tank!.Station!.AreaId == areaId);
        }
        if (stationId is not null)
        {
            query = query.Include(x => x.Tank)
            .Where(x => x.Tank!.StationId == stationId);
        }
        if (tankId is not null)
        {
            query = query.Where(x => x.TankId == tankId);
        }

        if (includeStation)
        {
            query = query.Include(x => x.Tank).ThenInclude(x => x!.Station);
        }
        else if (includeTank)
        {
            query = query.Include(x => x.Tank);
        }

        return await query.ToListAsync();
    }
}

