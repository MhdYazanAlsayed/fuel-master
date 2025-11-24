using System;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;
using FuelMaster.HeadOffice.Core.Interfaces.Database;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories;

public class FuelTypeRepository : IFuelTypeRepository
{
    private readonly FuelMasterDbContext _context;
    public FuelTypeRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }

    public FuelType Create(FuelType entity)
    {
        _context.FuelTypes.Add(entity);
        return entity;
    }

    public FuelType Delete(FuelType entity)
    {
        _context.FuelTypes.Remove(entity);
        return entity;
    }

    public async Task<FuelType?> DetailsAsync(int id)
    {
        return await _context.FuelTypes.FindAsync(id);
    }

    public async Task<List<FuelType>> GetAllAsync()
    {
        return await _context.FuelTypes.ToListAsync();
    }

    public async Task<(List<FuelType>, int)> GetPaginationAsync(int currentPage, int pageSize)
    {
        var count = await _context.FuelTypes.CountAsync();
        var data = await _context.FuelTypes.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
        return (data, count);
    }

    public FuelType Update(FuelType entity)
    {
        _context.FuelTypes.Update(entity);
        return entity;
    }
}
