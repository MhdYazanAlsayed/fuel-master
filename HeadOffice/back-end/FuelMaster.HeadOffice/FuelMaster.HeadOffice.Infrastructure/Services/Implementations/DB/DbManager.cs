using FuelMaster.HeadOffice.Application.Services.Interfaces.Database;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.DB;

public class DbManager : IDbManager
{
    public Task<string> BackupDatabase(string connectionString)
    {
        throw new NotImplementedException();
    }

    public async Task CreateDatabaseAsync(string connectionString)
    {
        var context = CreateDbContext(connectionString);

        await context.Database.MigrateAsync();
    }

    public Task<string> DeleteDatabase(string connectionString)
    {
        throw new NotImplementedException();
    }

    public Task<string> RestoreDatabase(string connectionString)
    {
        throw new NotImplementedException();
    }

    private FuelMasterDbContext CreateDbContext(string connectionString)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FuelMasterDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return new FuelMasterDbContext(optionsBuilder.Options);
    }
}