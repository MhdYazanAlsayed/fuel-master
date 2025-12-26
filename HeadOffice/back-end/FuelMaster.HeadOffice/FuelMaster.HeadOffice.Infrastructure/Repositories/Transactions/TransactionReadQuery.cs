using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Transactions;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories.Transactions;

public class TransactionReadQuery : ITransactionReadQuery
{
    private IQueryable<Transaction> _query;
    public TransactionReadQuery(IQueryable<Transaction> query)
    {
        _query = query;
    }

    public ITransactionReadQuery ApplyFilter(Scope scope, int? cityId, int? areaId, int? stationId)
    {
        if (scope == Scope.ALL)
            return this;

        if (scope == Scope.City)
        {
            _query = _query
            .Include(x => x.Station)
            .Where(x => x.Station!.CityId == cityId);
        }
        else if (scope == Scope.Area)
        {
            _query = _query
            .Include(x => x.Station)
            .Where(x => x.Station!.AreaId == areaId);
        }
        else if (scope == Scope.Station)
        {
            _query = _query
            .Include(x => x.Station)
            .Where(x => x.Station!.Id == stationId);
        }
        else if (scope == Scope.Self)
        {
            _query = _query.Take(0);
        }

        return this;
    }

    public async Task<List<Transaction>> GetAllAsync(DateTime from, DateTime to, int? areaId = null, int? cityId = null, int? stationId = null, int? nozzleId = null, int? pumpId = null, int? employeeId = null, bool includeNozzle = false, bool includeEmployee = false, bool includeStation = false, bool includePump = false)
    {
        var query = _query;
        query = query.Where(t => t.DateTime >= from && t.DateTime <= to);

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

        return await query.ToListAsync();

    }

    public async Task<(List<Core.Entities.Transaction> Data, int TotalCount)> GetPaginationAsync(int page,
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
        if (includePump)
        {
            _query = _query.Include(x => x.Nozzle)
            .ThenInclude(x => x!.Pump);
        }
        else if (includeNozzle)
        {
            _query = _query.Include(x => x.Nozzle);
        }

        if (includeEmployee)
        {
            _query = _query.Include(x => x.Employee);
        }

        if (includeStation)
        {
            _query = _query.Include(x => x.Station);
        }

        var count = await _query.CountAsync();
        var data = await _query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (data, count);
    }


}
