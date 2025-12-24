using FuelMaster.HeadOffice.Application.Services.Interfaces;
using System.Reflection;

namespace FuelMaster.HeadOffice.Infrastructure.Services.Implementations.Cache;

/// <summary>
/// Generic cache implementation for entities.
/// Provides caching operations for collections of entities with in-memory updates.
/// </summary>
/// <typeparam name="T">The entity type to cache</typeparam>
public class EntityCache<T> : IEntityCache<T> where T : class
{
    private readonly ICacheService _cacheService;
    private readonly string _cacheKey;
    private static readonly TimeSpan DefaultDuration = TimeSpan.FromMinutes(10);

    public EntityCache(ICacheService cacheService)
    {
        _cacheService = cacheService;
        _cacheKey = $"{typeof(T).Name}_All";
    }

    /// <summary>
    /// Gets all entities from cache
    /// </summary>
    public async Task<IEnumerable<T>?> GetAllEntitiesAsync()
    {
        return await _cacheService.GetAsync<List<T>>(_cacheKey);
    }

    /// <summary>
    /// Sets all entities in cache
    /// </summary>
    /// <param name="entities">The entities to cache</param>
    /// <param name="duration">Cache duration. Default is 10 minutes.</param>
    public async Task SetAllAsync(IEnumerable<T> entities, TimeSpan? duration = null)
    {
        var entitiesList = entities.ToList();
        var cacheDuration = duration ?? DefaultDuration;
        await _cacheService.SetAsync(_cacheKey, entitiesList, cacheDuration);
    }

    /// <summary>
    /// Updates cache after creating a new entity
    /// </summary>
    /// <param name="newEntity">The newly created entity</param>
    /// <param name="duration">Cache duration. Default is 10 minutes.</param>
    public async Task UpdateCacheAfterCreateAsync(T newEntity, TimeSpan? duration = null)
    {
        var cachedEntities = await GetAllEntitiesAsync();
        
        if (cachedEntities != null)
        {
            // Add the new entity to the cached collection
            var updatedEntities = cachedEntities.ToList();
            updatedEntities.Add(newEntity);
            
            var cacheDuration = duration ?? DefaultDuration;
            await _cacheService.SetAsync(_cacheKey, updatedEntities, cacheDuration);
        }
    }

    /// <summary>
    /// Updates cache after editing an entity
    /// </summary>
    /// <param name="updatedEntity">The updated entity</param>
    /// <param name="duration">Cache duration. Default is 10 minutes.</param>
    public async Task UpdateCacheAfterEditAsync(T updatedEntity, TimeSpan? duration = null)
    {
        var cachedEntities = await GetAllEntitiesAsync();
        
        if (cachedEntities != null)
        {
            var entityId = GetEntityId(updatedEntity);
            if (entityId == null)
            {
                // If we can't get the ID, refresh the entire cache
                return;
            }

            // Update the entity in the cached collection
            var updatedEntities = cachedEntities.ToList();
            var index = updatedEntities.FindIndex(e => GetEntityId(e) == entityId);
            
            if (index >= 0)
            {
                updatedEntities[index] = updatedEntity;
            }
            else
            {
                // Entity not found in cache, add it
                updatedEntities.Add(updatedEntity);
            }
            
            var cacheDuration = duration ?? DefaultDuration;
            await _cacheService.SetAsync(_cacheKey, updatedEntities, cacheDuration);
        }
    }

    /// <summary>
    /// Updates cache after deleting an entity by ID
    /// </summary>
    /// <param name="id">The ID of the deleted entity</param>
    /// <param name="duration">Cache duration. Default is 10 minutes.</param>
    public async Task UpdateCacheAfterDeleteAsync(int id, TimeSpan? duration = null)
    {
        var cachedEntities = await GetAllEntitiesAsync();
        
        if (cachedEntities != null)
        {
            // Remove the entity from the cached collection
            var updatedEntities = cachedEntities
                .Where(e => GetEntityId(e) != id)
                .ToList();
            
            var cacheDuration = duration ?? DefaultDuration;
            await _cacheService.SetAsync(_cacheKey, updatedEntities, cacheDuration);
        }
    }

    /// <summary>
    /// Gets the ID property value from an entity using reflection
    /// </summary>
    private int? GetEntityId(T entity)
    {
        if (entity == null)
            return null;

        // Try to get the Id property using reflection
        var idProperty = typeof(T).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
        
        if (idProperty != null)
        {
            var idValue = idProperty.GetValue(entity);
            
            // Handle different ID types (int, long, etc.)
            if (idValue is int intId)
                return intId;
            
            if (idValue is long longId)
                return (int)longId; // Cast long to int for comparison
            
            if (idValue != null && int.TryParse(idValue.ToString(), out var parsedId))
                return parsedId;
        }

        return null;
    }

    public async Task DeleteEntityFromCacheAsync(int id)
    {
        await _cacheService.RemoveAsync(_cacheKey);
    }

    public async Task DeleteAllEntitiesFromCacheAsync()
    {
        await _cacheService.RemoveAsync(_cacheKey);
    }
}

