using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories;

public class FuelMasterAreaOfAccessRepository : IFuelMasterAreaOfAccessRepository
{
    private readonly FuelMasterDbContext _context;

    public FuelMasterAreaOfAccessRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }

    public FuelMasterAreaOfAccess Create(FuelMasterAreaOfAccess entity)
    {
        _context.FuelMasterAreasOfAccess.Add(entity);
        return entity;
    }

    public FuelMasterAreaOfAccess Delete(FuelMasterAreaOfAccess entity)
    {
        _context.FuelMasterAreasOfAccess.Remove(entity);
        return entity;
    }

    public async Task<FuelMasterAreaOfAccess?> DetailsAsync(int id)
    {
        return await _context.FuelMasterAreasOfAccess.FindAsync(id);
    }

    public async Task<List<FuelMasterAreaOfAccess>> GetAllAsync()
    {
        return await _context.FuelMasterAreasOfAccess.ToListAsync();
    }

    public async Task<(List<FuelMasterAreaOfAccess>, int)> GetPaginationAsync(int currentPage, int pageSize)
    {
        var count = await _context.FuelMasterAreasOfAccess.CountAsync();
        var data = await _context.FuelMasterAreasOfAccess
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (data, count);
    }

    public FuelMasterAreaOfAccess Update(FuelMasterAreaOfAccess entity)
    {
        _context.FuelMasterAreasOfAccess.Update(entity);
        return entity;
    }
}

