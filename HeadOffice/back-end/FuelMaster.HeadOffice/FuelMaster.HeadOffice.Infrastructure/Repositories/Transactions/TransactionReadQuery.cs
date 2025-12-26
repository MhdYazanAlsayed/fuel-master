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

    public async Task<(List<Core.Entities.Transaction> Data, int TotalCount)> GetPaginationAsync(int page, int pageSize, bool includeNozzle = false, bool includeEmployee = false, bool includeStation = false, bool includePump = false)
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
