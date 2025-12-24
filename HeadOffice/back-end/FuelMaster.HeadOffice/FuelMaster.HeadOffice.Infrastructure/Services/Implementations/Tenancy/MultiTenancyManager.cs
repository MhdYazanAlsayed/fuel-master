using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.Tanents.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Tanents.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Database;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;


namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.Tenancy;

public class MultiTenancyManager : IMultiTenancyManager
{
    private readonly IDbManager _dbManager;
    private readonly IConnectionString _connectionStringService;

    public MultiTenancyManager(IDbManager dbManager, 
    IConnectionString connectionStringService)
    {
        _dbManager = dbManager;
        _connectionStringService = connectionStringService;
    }
    public Task<ResultDto> BackupDatabaseForTenantAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ResultDto<DatabaseTanentResult>> CreateDatabaseForTenantAsync(CreateDatabaseForTanentDto createDatabaseForTanentDto)
    {
        try 
        {
            // Generate database name
            var result = GenerateConnectionsString(createDatabaseForTanentDto.TenantName);
            await _dbManager.CreateDatabaseAsync(result.ConnectionString);

            return Result.Success(new DatabaseTanentResult
            {
                DatabaseName = result.DatabaseName,
                ConnectionString = result.ConnectionString,
            });
        }
        catch 
        {
            // TODO : Write error throght logger service.
            return Result.Failure<DatabaseTanentResult>("Failed to create database for tenant");
        }
        
    }

    public Task<ResultDto> DeleteDatabaseForTenantAsync()
    {
        throw new NotImplementedException();
    }

    // private string GenerateStrongPassword(int length = 32)
    // {
    //     const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    //     const string lowercase = "abcdefghijklmnopqrstuvwxyz";
    //     const string digits = "0123456789";
    //     const string specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

    //     var allChars = uppercase + lowercase + digits + specialChars;
    //     var password = new StringBuilder(length);

    //     using (var rng = RandomNumberGenerator.Create())
    //     {
    //         // Ensure at least one character from each category
    //         password.Append(uppercase[GetRandomInt(rng, uppercase.Length)]);
    //         password.Append(lowercase[GetRandomInt(rng, lowercase.Length)]);
    //         password.Append(digits[GetRandomInt(rng, digits.Length)]);
    //         password.Append(specialChars[GetRandomInt(rng, specialChars.Length)]);

    //         // Fill the rest with random characters
    //         for (int i = password.Length; i < length; i++)
    //         {
    //             password.Append(allChars[GetRandomInt(rng, allChars.Length)]);
    //         }
    //     }

    //     // Shuffle the password to avoid predictable pattern
    //     return ShuffleString(password.ToString());
    // }

    // private int GetRandomInt(RandomNumberGenerator rng, int maxValue)
    // {
    //     var bytes = new byte[4];
    //     rng.GetBytes(bytes);
    //     var value = BitConverter.ToUInt32(bytes, 0);
    //     return (int)(value % maxValue);
    // }

    // private string ShuffleString(string input)
    // {
    //     var characters = input.ToCharArray();
    //     using (var rng = RandomNumberGenerator.Create())
    //     {
    //         for (int i = characters.Length - 1; i > 0; i--)
    //         {
    //             var randomIndex = GetRandomInt(rng, i + 1);
    //             (characters[i], characters[randomIndex]) = (characters[randomIndex], characters[i]);
    //         }
    //     }
    //     return new string(characters);
    // }

    // <Summary>
    // Generates a connection string for a tenant
    // </Summary>
    // <returns>
    // A tuple containing the database name and the connection string
    // </returns>
    private (string DatabaseName, string ConnectionString) GenerateConnectionsString(string tenantName)
    {
        var dbName = $"{tenantName.ToLower()}-{Guid.NewGuid()}";
        var connectionString = _connectionStringService.GetConnectionString(dbName);
        
        return (dbName, connectionString);
    }
}