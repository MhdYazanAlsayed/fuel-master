using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly FuelMasterDbContext _context;

    public EmployeeRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }

    public Employee Create(Employee entity)
    {
        _context.Employees.Add(entity);
        return entity;
    }

    public Employee Delete(Employee entity)
    {
        _context.Employees.Remove(entity);
        return entity;
    }

    public async Task<Employee?> DetailsAsync(int id, bool includeRole = false, bool includeStation = false, bool includeArea = false, bool includeCity = false, bool includeUser = false, bool ignoreQueryFilter = false)
    {
        var query = _context.Employees.AsQueryable();

        if (ignoreQueryFilter)
        {
            query = query.IgnoreQueryFilters();
        }

        if (includeRole)
        {
            query = query.Include(e => e.Role);
        }

        if (includeStation)
        {
            query = query.Include(e => e.Station);
        }

        if (includeArea)
        {
            query = query.Include(e => e.Area);
        }

        if (includeCity)
        {
            query = query.Include(e => e.City);
        }

        if (includeUser)
        {
            query = query.Include(e => e.User);
        }

        return await query.SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<(List<Employee>, int)> GetPaginationAsync(int page, int pageSize, bool includeRole = false, bool includeStation = false, bool includeArea = false, bool includeCity = false, bool includeUser = false, bool ignoreQueryFilter = false)
    {
        var query = _context.Employees.AsQueryable();
        if (ignoreQueryFilter)
        {
            query = query.IgnoreQueryFilters();
        }

        if (includeRole)
        {
            query = query.Include(e => e.Role);
        }

        if (includeStation)
        {
            query = query.Include(e => e.Station);
        }

        if (includeArea)
        {
            query = query.Include(e => e.Area);
        }

        if (includeCity)
        {
            query = query.Include(e => e.City);
        }

        if (includeUser)
        {
            query = query.Include(e => e.User);
        }

        var count = await query.CountAsync();
        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, count);
    }

    public Employee Update(Employee entity)
    {
        _context.Employees.Update(entity);
        return entity;
    }

    public async Task<List<Employee>> GetAllAsync(bool includeRole = false, bool includeStation = false, bool includeArea = false, bool includeCity = false, bool includeUser = false, bool ignoreFilter = false)
    {
        var query = _context.Employees.AsQueryable();

        if (ignoreFilter)
        {
            query = query.IgnoreQueryFilters();
        }

        if (includeRole)
        {
            query = query.Include(e => e.Role);
        }

        if (includeStation)
        {
            query = query.Include(e => e.Station);
        }

        if (includeArea)
        {
            query = query.Include(e => e.Area);
        }

        if (includeCity)
        {
            query = query.Include(e => e.City);
        }

        if (includeUser)
        {
            query = query.Include(e => e.User);
        }

        return await query.ToListAsync();
    }

    public async Task<List<Employee>> GetAllByCityId (int cityId)
    {
        return await _context.Employees
        .Include(x => x.Station)
        .Include(x => x.Area)
        .Where(x => x.Station != null && x.Station.CityId == cityId)
        .Where(x => x.Area != null && x.Area.CityId == cityId)
        .ToListAsync();
    }

    public async Task<List<Employee>> GetAllByAreaId (int areaId)
    {
        return await _context.Employees
        .Include(x => x.Station)
        .Include(x => x.Area)
        .Where(x => x.Station != null && x.Station.AreaId == areaId)
        .ToListAsync();
    }
}

