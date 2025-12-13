using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories;

public class FuelMasterRoleRepository : IFuelMasterRoleRepository
{
    private readonly FuelMasterDbContext _context;

    public FuelMasterRoleRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }

    public FuelMasterRole Create(FuelMasterRole entity)
    {
        _context.FuelMasterRoles.Add(entity);
        return entity;
    }

    public FuelMasterRole Delete(FuelMasterRole entity)
    {
        _context.FuelMasterRoles.Remove(entity);
        return entity;
    }

    public async Task<FuelMasterRole?> DetailsAsync(int id)
    {
        return await _context.FuelMasterRoles.FindAsync(id);
    }

    public async Task<List<FuelMasterRole>> GetAllAsync(bool includeAreasOfAccess = false)
    {
        var query = _context.FuelMasterRoles.AsQueryable();

        if (includeAreasOfAccess)
        {
            query = query.Include(r => r.AreasOfAccess);
        }

        return await query.ToListAsync();
    }

    public async Task<(List<FuelMasterRole>, int)> GetPaginationAsync(int page, int pageSize, bool includeAreasOfAccess = false)
    {
        var query = _context.FuelMasterRoles.AsQueryable();

        if (includeAreasOfAccess)
        {
            query = query.Include(r => r.AreasOfAccess);
        }

        var count = await query.CountAsync();
        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, count);
    }

    public FuelMasterRole Update(FuelMasterRole entity)
    {
        _context.FuelMasterRoles.Update(entity);
        return entity;
    }
}

