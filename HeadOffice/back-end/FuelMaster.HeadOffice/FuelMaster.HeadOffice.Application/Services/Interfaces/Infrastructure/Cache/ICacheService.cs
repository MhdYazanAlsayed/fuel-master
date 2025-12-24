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
}