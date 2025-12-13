using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Database;

public interface IConnectionString : IScopedDependency
{
    /// <summary>
    /// This method is used to generate connection string using variables in configuration file and for specific tenant.
    /// </summary>
    /// <param name="databaseName">The name of the database to generate connection string for.</param>
    /// <returns>A string that represents the connection string.</returns>
    string GetConnectionString(string databaseName);
}