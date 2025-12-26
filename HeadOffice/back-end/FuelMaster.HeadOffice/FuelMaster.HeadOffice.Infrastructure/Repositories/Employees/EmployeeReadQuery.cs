using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.ResultQuery;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Employees;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories.Employees;

public class EmployeeReadQuery : IEmployeeReadQuery
{
    private IQueryable<Employee> _query;
    public EmployeeReadQuery(IQueryable<Employee> query)
    {
        _query = query;
    }

    public IEmployeeReadQuery ApplyFilter(Scope scope, int? cityId, int? areaId, int? stationId)
    {
        if (scope == Scope.ALL)
            return this;

        if (scope == Scope.City)
        {
            _query = _query.Where(x => 
                x.Scope == Scope.Self || x.Scope == Scope.Station && x.Station!.CityId == cityId || 
                (x.Scope == Scope.Area && x.Area!.CityId == cityId)
            );
        }
        else if (scope == Scope.Area)
        {
            _query = _query.Where(x => 
                x.Scope == Scope.Self || x.Scope == Scope.Station && x.Station!.AreaId == areaId 
            );
        } 
        else if (scope == Scope.Station)
        {
            _query = _query.Where(x => 
                x.Scope == Scope.Self && x.Station!.Id == stationId
            );
        }
        else if (scope == Scope.Self)
        {
            _query = _query.Take(0);
        }

        return this;
    }

    // We violate DRY Principle here, but it's ok because its an MVP.
    public async Task<List<Employee>> GetAllAsync(bool includeRole = false, bool includeStation = false, bool includeArea = false, bool includeCity = false, bool includeUser = false)
    {
        if (includeRole)
        {
            _query = _query.Include(e => e.Role);
        }

        if (includeStation)
        {
            _query = _query.Include(e => e.Station);
        }

        if (includeArea)
        {
            _query = _query.Include(e => e.Area);
        }

        if (includeCity)
        {
            _query = _query.Include(e => e.City);
        }

        if (includeUser)
        {
            _query = _query.Include(e => e.User);
        }

        return await _query.ToListAsync();
    }

    public async Task<(List<Employee>, int)> GetPaginationAsync(int page, int pageSize, bool includeRole = false, bool includeStation = false, bool includeArea = false, bool includeCity = false, bool includeUser = false, bool ignoreQueryFilter = false)
    {
        if (includeRole)
        {
            _query = _query.Include(e => e.Role);
        }

        if (includeStation)
        {
            _query = _query.Include(e => e.Station);
        }

        if (includeArea)
        {
            _query = _query.Include(e => e.Area);
        }

        if (includeCity)
        {
            _query = _query.Include(e => e.City);
        }

        if (includeUser)
        {
            _query = _query.Include(e => e.User);
        }

        var count = await _query.CountAsync();
        var data = await _query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, count);
    }

   
}