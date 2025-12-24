using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Database;

public interface IDbManager : IScopedDependency
{
    /// <summary>
    /// This method is used to create a database.
    /// </summary>
    /// <param name="connectionString">The connection string to use for the database.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="Exception">Thrown when the request fails.</exception>  
    Task CreateDatabaseAsync (string connectionString);
}