using FuelMaster.HeadOffice.Core.Contracts.Caching;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace FuelMaster.HeadOffice.ApplicationService.Caching;

/// <summary>
/// Generic implementation of entity cache service.
/// Caches entities using nameof(T) for type-safe cache keys.
/// </summary>
/// <typeparam name="T">The entity type to cache</typeparam>
public class EntityCache<T> : IEntityCache<T> where T : class
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<EntityCache<T>> _logger;
    private readonly string _cacheKey;
    private readonly PropertyInfo? _idProperty;
    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(10);

    public EntityCache(ICacheService cacheService, ILogger<EntityCache<T>> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
        
        // Use nameof(T) to generate cache key - e.g., "Station_All" for Station type
        var typeName = typeof(T).Name;
        _cacheKey = $"{typeName}_All";
        
        // Find Id property using reflection (works for Entity<T> and AggregateRoot<T>)
        _idProperty = typeof(T).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
    }

    public async Task<IEnumerable<T>?> GetAllEntitiesAsync()
    {
        _logger.LogDebug("Getting all {TypeName} entities from cache", typeof(T).Name);
        
        var cachedEntities = await _cacheService.GetAsync<IEnumerable<T>>(_cacheKey);
        
        if (cachedEntities != null)
        {
            _logger.LogDebug("Retrieved {Count} {TypeName} entities from cache", cachedEntities.Count(), typeof(T).Name);
        }
        else
        {
            _logger.LogDebug("{TypeName} entities not found in cache", typeof(T).Name);
        }
        
        return cachedEntities;
    }

    public async Task SetAllAsync(IEnumerable<T> entities, TimeSpan? duration = null)
    {
        var cacheDuration = duration ?? DefaultCacheDuration;
        _logger.LogDebug("Caching {Count} {TypeName} entities with duration: {Duration} minutes", 
            entities.Count(), typeof(T).Name, cacheDuration.TotalMinutes);
        await _cacheService.SetAsync(_cacheKey, entities, cacheDuration);
    }

    public async Task UpdateCacheAfterCreateAsync(T newEntity, TimeSpan? duration = null)
    {
        var entityId = GetEntityId(newEntity);
        var cacheDuration = duration ?? DefaultCacheDuration;
        _logger.LogDebug("Updating {TypeName} cache after creating entity with ID: {Id}", typeof(T).Name, entityId);

        var cached = await _cacheService.GetAsync<IEnumerable<T>>(_cacheKey);
        if (cached is null)
        {
            _logger.LogDebug("{TypeName} cache not found, skipping cache update", typeof(T).Name);
            return;
        }

        var updatedList = cached.ToList();
        updatedList.Add(newEntity);
        await _cacheService.SetAsync(_cacheKey, updatedList, cacheDuration);
        
        _logger.LogDebug("Updated {TypeName} cache with new entity (ID: {Id})", typeof(T).Name, entityId);
    }

    public async Task UpdateCacheAfterEditAsync(T updatedEntity, TimeSpan? duration = null)
    {
        var entityId = GetEntityId(updatedEntity);
        var cacheDuration = duration ?? DefaultCacheDuration;
        _logger.LogDebug("Updating {TypeName} cache after editing entity with ID: {Id}", typeof(T).Name, entityId);

        var cached = await _cacheService.GetAsync<IEnumerable<T>>(_cacheKey);
        if (cached is null)
        {
            _logger.LogDebug("{TypeName} cache not found, skipping cache update", typeof(T).Name);
            return;
        }

        var updatedList = cached.ToList();
        var index = updatedList.FindIndex(e => GetEntityId(e)!.Equals(entityId));
        if (index >= 0)
        {
            updatedList[index] = updatedEntity;
            await _cacheService.SetAsync(_cacheKey, updatedList, cacheDuration);
            _logger.LogDebug("Updated {TypeName} cache with edited entity (ID: {Id})", typeof(T).Name, entityId);
        }
        else
        {
            _logger.LogWarning("{TypeName} entity with ID {Id} not found in cache for update", typeof(T).Name, entityId);
        }
    }

    public async Task UpdateCacheAfterDeleteAsync(int id, TimeSpan? duration = null)
    {
        var cacheDuration = duration ?? DefaultCacheDuration;
        _logger.LogDebug("Updating {TypeName} cache after deleting entity with ID: {Id}", typeof(T).Name, id);

        var cached = await _cacheService.GetAsync<IEnumerable<T>>(_cacheKey);
        if (cached is null)
        {
            _logger.LogDebug("{TypeName} cache not found, skipping cache update", typeof(T).Name);
            return;
        }

        var updatedList = cached.Where(e => !GetEntityId(e)!.Equals(id)).ToList();
        await _cacheService.SetAsync(_cacheKey, updatedList, cacheDuration);
        
        _logger.LogDebug("Removed {TypeName} entity (ID: {Id}) from cache", typeof(T).Name, id);
    }

    /// <summary>
    /// Gets the Id property value from an entity using reflection
    /// </summary>
    private object? GetEntityId(T entity)
    {
        if (_idProperty == null)
        {
            _logger.LogWarning("Id property not found on {TypeName}", typeof(T).Name);
            return null;
        }

        return _idProperty.GetValue(entity);
    }
}

