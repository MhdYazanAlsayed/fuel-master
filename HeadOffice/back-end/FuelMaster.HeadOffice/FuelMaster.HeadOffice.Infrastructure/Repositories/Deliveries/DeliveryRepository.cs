using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Deliveries;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
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

    public IDeliveryReadQuery AsReadQuery()
    {
        throw new NotImplementedException();
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

    public async Task<(List<Delivery>, int)> GetPaginationAsync(int page, int pageSize, bool includeStation = false, bool includeTank = false)
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
}

