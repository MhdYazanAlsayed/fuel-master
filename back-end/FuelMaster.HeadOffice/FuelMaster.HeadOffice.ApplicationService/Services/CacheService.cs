using FuelMaster.HeadOffice.Core.Configurations;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FuelMaster.HeadOffice.ApplicationService.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly CacheConfiguration _cacheConfig;
        private readonly ILogger<CacheService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string TenantIdItemKey = "TenantId";

        public CacheService(IMemoryCache cache, IOptions<CacheConfiguration> cacheConfig, 
            ILogger<CacheService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _cacheConfig = cacheConfig.Value;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<T?> GetAsync<T>(string key) where T : class
        {
            var tenantAwareKey = GetTenantAwareKey(key);
            _logger.LogDebug("Attempting to get cached value for key: {Key} (tenant-aware: {TenantAwareKey})", key, tenantAwareKey);
            
            if (_cache.TryGetValue(tenantAwareKey, out T? cachedValue))
            {
                _logger.LogDebug("Cache hit for key: {Key}", tenantAwareKey);
                return Task.FromResult(cachedValue);
            }

            _logger.LogDebug("Cache miss for key: {Key}", tenantAwareKey);
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
            
            _logger.LogDebug("Cached value for key: {Key} (tenant-aware: {TenantAwareKey}) with duration: {Duration} minutes", 
                key, tenantAwareKey, cacheDuration.TotalMinutes);
            
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            var tenantAwareKey = GetTenantAwareKey(key);
            _cache.Remove(tenantAwareKey);
            _logger.LogDebug("Removed cache entry for key: {Key} (tenant-aware: {TenantAwareKey})", key, tenantAwareKey);
            return Task.CompletedTask;
        }

        public Task RemoveByPatternAsync(string pattern)
        {
            var tenantId = GetCurrentTenantId();
            var tenantAwarePattern = string.IsNullOrWhiteSpace(tenantId) ? pattern : $"{tenantId}_{pattern}";
            
            _logger.LogDebug("Removing cache entries matching pattern: {Pattern} (tenant-aware: {TenantAwarePattern})", 
                pattern, tenantAwarePattern);
            
            // Get all cache keys that match the pattern
            var cacheKeys = GetCacheKeysByPattern(tenantAwarePattern);
            
            foreach (var key in cacheKeys)
            {
                _cache.Remove(key);
                _logger.LogDebug("Removed cache entry: {Key}", key);
            }
            
            _logger.LogInformation("Removed {Count} cache entries matching pattern: {Pattern} for tenant: {TenantId}", 
                cacheKeys.Count, pattern, tenantId);
            
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
                _logger.LogWarning(ex, "Error retrieving cache keys for pattern: {Pattern}", pattern);
            }
            
            return cacheKeys;
        }

        // Helper methods for specific cache operations
        public Task SetFuelMasterGroupsAsync<T>(T value) where T : class
        {
            return SetAsync("FuelMasterGroups_All", value, 
                TimeSpan.FromMinutes(_cacheConfig.FuelMasterGroupsDurationMinutes));
        }

        public Task SetPaginationAsync<T>(int page, T value) where T : class
        {
            var key = $"FuelMasterGroups_Page_{page}";
            return SetAsync(key, value, 
                TimeSpan.FromMinutes(_cacheConfig.PaginationDurationMinutes));
        }

        public Task SetDetailsAsync<T>(int id, T value) where T : class
        {
            var key = $"FuelMasterGroup_Details_{id}";
            return SetAsync(key, value, 
                TimeSpan.FromMinutes(_cacheConfig.DetailsDurationMinutes));
        }

        public Task RemoveAllFuelMasterGroupsAsync()
        {
            return RemoveByPatternAsync("FuelMasterGroups");
        }

        // Tenant context helper methods
        private string? GetCurrentTenantId()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context?.Items.TryGetValue(TenantIdItemKey, out var tenantId) == true)
            {
                return tenantId?.ToString();
            }
            return null;
        }

        private string GetTenantAwareKey(string baseKey)
        {
            var tenantId = GetCurrentTenantId();
            return string.IsNullOrWhiteSpace(tenantId) 
                ? baseKey 
                : $"{tenantId}_{baseKey}";
        }
    }
}
