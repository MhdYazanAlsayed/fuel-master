using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Deliveries;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories.Deliveries;

public class DeliveryReadQuery : IDeliveryReadQuery
{
    private IQueryable<Delivery> _query;
    public DeliveryReadQuery(IQueryable<Delivery> query)
    {
        _query = query;
    }

    public IDeliveryReadQuery ApplyFilter(Scope scope, int? cityId, int? areaId, int? stationId)
    {
        if (scope == Scope.ALL)
            return this;

        if (scope == Scope.City)
        {
            _query = _query.Where(d => d.Tank!.Station!.CityId == cityId);
        }
        else if (scope == Scope.Area)
        {
            _query = _query.Where(d => d.Tank!.Station!.AreaId == areaId);
        }
        else if (scope == Scope.Station || scope == Scope.Self)
        {
            _query = _query.Where(d => d.Tank!.StationId == stationId);
        }

        return this;
    }

    public async Task<(List<Delivery>, int)> GetPaginationAsync(int page, int pageSize, bool includeStation = false, bool includeTank = false)
    {
        if (includeStation)
        {
            _query = _query
            .Include(d => d.Tank)
            .ThenInclude(x => x!.Station);
        }
        else if (includeTank)
        {
            _query = _query.Include(d => d.Tank);
        }

        var count = await _query.CountAsync();
        var data = await _query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, count);
    }
}