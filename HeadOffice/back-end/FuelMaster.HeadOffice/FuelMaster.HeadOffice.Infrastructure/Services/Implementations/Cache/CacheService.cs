using FuelMaster.HeadOffice.Application.Services.Interfaces;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;
using FuelMaster.HeadOffice.Infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.Cache;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly CacheConfiguration _cacheConfig;
    private readonly ICurrentTenant _currentTenant;
    public CacheService(IMemoryCache cache, IOptions<CacheConfiguration> cacheConfig, 
        ICurrentTenant currentTenant)
    {
        _cache = cache;
        _cacheConfig = cacheConfig.Value;
        _currentTenant = currentTenant;
    }

    public Task<T?> GetAsync<T>(string key) where T : class
    {
        var tenantAwareKey = GetTenantAwareKey(key);
        
        if (_cache.TryGetValue(tenantAwareKey, out T? cachedValue))
        {
            return Task.FromResult(cachedValue);
        }

        return Task.FromResult<T?>(null);
    }

    public async Task<T?> GetAndStoreAsync<T>(string key, Func<Task<T>> createValue, TimeSpan? duration = null) where T : class
    {
        var tenantAwareKey = GetTenantAwareKey(key);

        if (_cache.TryGetValue(tenantAwareKey, out T? cachedValue))
        {
            return cachedValue;
        }

        var value = await createValue();
        await SetAsync(tenantAwareKey, value, duration ?? TimeSpan.FromMinutes(_cacheConfig.DefaultDurationMinutes));
        return value;
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? duration = null) where T : class
    {
        var tenantAwareKey = GetTenantAwareKey(key);
        var cacheDuration = duration ?? TimeSpan.FromMinutes(_cacheConfig.DefaultDurationMinutes);
        
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration,
            Priority = CacheItemPriority.Normal
        };

        _cache.Set(tenantAwareKey, value, cacheOptions);
        
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        var tenantAwareKey = GetTenantAwareKey(key);
        _cache.Remove(tenantAwareKey);
        return Task.CompletedTask;
    }

    public Task RemoveByPatternAsync(string pattern)
    {
        var tenantId = GetCurrentTenantId();
        var tenantAwarePattern = string.IsNullOrWhiteSpace(tenantId) ? pattern : $"{tenantId}_{pattern}";
        
        // Get all cache keys that match the pattern
        var cacheKeys = GetCacheKeysByPattern(tenantAwarePattern);
        
        foreach (var key in cacheKeys)
        {
            _cache.Remove(key);
        }
        
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string key)
    {
        var tenantAwareKey = GetTenantAwareKey(key);
        return Task.FromResult(_cache.TryGetValue(tenantAwareKey, out _));
    }

    private List<string> GetCacheKeysByPattern(string pattern)
    {
        var cacheKeys = new List<string>();
        
        try
        {
            // Use reflection to access the internal cache entries
            var field = typeof(MemoryCache).GetField("_coherentState", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (field != null)
            {
                var coherentState = field.GetValue(_cache);
                var entriesCollection = coherentState?.GetType()
                    .GetProperty("EntriesCollection", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (entriesCollection != null)
                {
                    var entries = entriesCollection.GetValue(coherentState);
                    if (entries is IDictionary<string, object> entriesDict)
                    {
                        foreach (var key in entriesDict.Keys)
                        {
                            if (key.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                            {
                                cacheKeys.Add(key);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving cache keys for pattern: {pattern}", ex);
        }
        
        return cacheKeys;
    }
    
    // Tenant context helper methods
    private string? GetCurrentTenantId()
    {
        return _currentTenant.TenantId.ToString();
    }

    private string GetTenantAwareKey(string baseKey)
    {
        var tenantId = GetCurrentTenantId();
        return string.IsNullOrWhiteSpace(tenantId) 
            ? baseKey 
            : $"{tenantId}_{baseKey}";
    }
}