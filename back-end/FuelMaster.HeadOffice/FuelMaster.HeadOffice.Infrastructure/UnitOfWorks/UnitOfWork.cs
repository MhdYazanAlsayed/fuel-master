using System;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Interfaces.Database;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace FuelMaster.HeadOffice.Infrastructure.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly FuelMasterDbContext _context;
    private IDbContextTransaction? _transaction;
    public UnitOfWork(IContextFactory<FuelMasterDbContext> context)
    {
        _context = context.CurrentContext;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction is null)
            throw new InvalidOperationException("No transaction to commit");

        await _transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction is null)
            throw new InvalidOperationException("No transaction to rollback");
            
        await _transaction.RollbackAsync();
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
