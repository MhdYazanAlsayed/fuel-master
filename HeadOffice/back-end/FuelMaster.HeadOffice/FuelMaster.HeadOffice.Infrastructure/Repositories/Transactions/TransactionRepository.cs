using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Transactions;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Repositories.Transactions;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly FuelMasterDbContext _context;

    public TransactionRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }

    public ITransactionReadQuery UseScopeFilter()
    {
        return new TransactionReadQuery(_context.Transactions.AsQueryable());
    }

    public Transaction Create(Transaction entity)
    {
        _context.Transactions.Add(entity);
        return entity;
    }

    public Transaction Delete(Transaction entity)
    {
        _context.Transactions.Remove(entity);
        return entity;
    }

    public async Task<Transaction?> DetailsAsync(long id, bool includeNozzle = false, bool includeEmployee = false, bool includeStation = false, bool includePump = false)
    {
        var query = _context.Transactions.AsQueryable();

        if (includePump)
        {
            query = query
                .Include(t => t.Nozzle)
                .ThenInclude(n => n!.Pump);
        }
        else if (includeNozzle)
        {
            query = query.Include(t => t.Nozzle);
        }

        if (includeEmployee)
        {
            query = query.Include(t => t.Employee);
        }

        if (includeStation)
        {
            query = query.Include(t => t.Station);
        }

        return await query.SingleOrDefaultAsync(t => t.Id == id);
    }

    // We need to avoid counting all records by SQL query
    // Instead of that we want to store the count in specific field and get it directly 
    // without doing this heavy operation.
    public async Task<(List<Transaction>, int)> GetPaginationAsync(
        int page, 
        int pageSize, 
        // Filters,
        int? areaId = null,
        int? cityId = null,
        int? stationId = null,
        int? nozzleId = null,
        int? pumpId = null,
        int? employeeId = null,
        DateTime? from = null,
        DateTime? to = null,
        
        // Includes
        bool includeNozzle = false, 
        bool includeEmployee = false, 
        bool includeStation = false, 
        bool includePump = false)
    {
        var query = _context.Transactions.AsQueryable();

        if (areaId is not null)
        {
            query = query
            .Include(x => x.Station)
            .Where(t => t.Station!.AreaId == areaId);
        }
        if (cityId is not null)
        {
            query = query
            .Include(x => x.Station)
            .Where(t => t.Station!.CityId == cityId);
        }
        if (stationId is not null)
        {
            query = query
            .Include(x => x.Station)
            .Where(t => t.Station!.Id == stationId);
        }
        if (nozzleId is not null)
        {
            query = query
            .Include(x => x.Nozzle)
            .Where(t => t.Nozzle!.Id == nozzleId);
        }
        if (pumpId is not null)
        {
            query = query
            .Include(x => x.Nozzle)
            .Where(t => t.Nozzle!.PumpId == pumpId);
        }
        if (employeeId is not null)
        {
            query = query
            .Include(x => x.Employee)
            .Where(t => t.Employee!.Id == employeeId);
        }
        if (from is not null)
        {
            query = query.Where(t => t.DateTime >= from);
        }
        if (to is not null)
        {
            query = query.Where(t => t.DateTime <= to);
        }

        if (includePump)
        {
            query = query
                .Include(t => t.Nozzle)
                .ThenInclude(n => n!.Pump);
        }
        else if (includeNozzle)
        {
            query = query.Include(t => t.Nozzle);
        }

        if (includeEmployee)
        {
            query = query.Include(t => t.Employee);
        }

        if (includeStation)
        {
            query = query.Include(t => t.Station);
        }

        var count = await query.CountAsync();
        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, count);
    }

    public Transaction Update(Transaction entity)
    {
        _context.Transactions.Update(entity);
        return entity;
    }
}

