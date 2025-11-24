namespace FuelMaster.HeadOffice.Application.Services.Interfaces;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

public interface ICacheService : IScopedDependency
{
    Task<T?> GetAsync<T>(string key) where T : class;
    Task SetAsync<T>(string key, T value, TimeSpan? duration = null) where T : class;
    Task RemoveAsync(string key);
    Task RemoveByPatternAsync(string pattern);
    Task<bool> ExistsAsync(string key);
    Task<T?> GetAndStoreAsync<T>(string key, Func<Task<T>> createValue, TimeSpan? duration = null) where T : class;
    
    // // Helper methods for specific cache operations
    // Task SetFuelMasterGroupsAsync<T>(T value) where T : class;
    // Task SetPaginationAsync<T>(int page, T value) where T : class;
    // Task SetDetailsAsync<T>(int id, T value) where T : class;
    // Task RemoveAllFuelMasterGroupsAsync();
}