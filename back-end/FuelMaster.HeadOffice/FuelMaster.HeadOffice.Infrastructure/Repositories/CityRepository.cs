using FuelMaster.HeadOffice.Core.Interfaces.Database;
using FuelMaster.HeadOffice.Core.Interfaces.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories;

public class CityRepository : ICityRepository
{
    private readonly FuelMasterDbContext _context;
    public CityRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }
  
    public async Task<City> CreateAsync(City entity)
    {
        _context.Cities.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<City> DeleteAsync(City entity)
    {
        _context.Cities.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<City?> DetailsAsync(int id)
    {
        return await _context.Cities.FindAsync(id);
    }

    public async Task<List<City>> GetAllAsync()
    {
        return await _context.Cities.ToListAsync();
    }

    public async Task<(List<City>, int)> GetPaginationAsync(int page, int pageSize)
    {
        var count = await _context.Cities.CountAsync();
        var data = await _context.Cities.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (data, count);
    }

    public async Task<City> UpdateAsync(City entity)
    {
        _context.Cities.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}