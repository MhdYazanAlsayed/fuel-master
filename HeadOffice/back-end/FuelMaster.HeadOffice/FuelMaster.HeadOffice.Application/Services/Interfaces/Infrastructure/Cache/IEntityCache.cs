using FuelMaster.HeadOffice.Core.Interfaces.Markers;

/// <summary>
/// Generic cache interface for entities.
/// Provides caching operations for collections of entities.
/// </summary>
/// <typeparam name="T">The entity type to cache</typeparam>
public interface IEntityCache<T> : IScopedDependency where T : class
{
    /// <summary>
    /// Gets all entities from cache
    /// </summary>
    Task<IEnumerable<T>?> GetAllEntitiesAsync();
    
    /// <summary>
    /// Sets all entities in cache
    /// </summary>
    /// <param name="entities">The entities to cache</param>
    /// <param name="duration">Cache duration. Default is 10 minutes.</param>
    Task SetAllAsync(IEnumerable<T> entities, TimeSpan? duration = null);
    
    /// <summary>
    /// Updates cache after creating a new entity
    /// </summary>
    /// <param name="newEntity">The newly created entity</param>
    /// <param name="duration">Cache duration. Default is 10 minutes.</param>
    Task UpdateCacheAfterCreateAsync(T newEntity, TimeSpan? duration = null);
    
    /// <summary>
    /// Updates cache after editing an entity
    /// </summary>
    /// <param name="updatedEntity">The updated entity</param>
    /// <param name="duration">Cache duration. Default is 10 minutes.</param>
    Task UpdateCacheAfterEditAsync(T updatedEntity, TimeSpan? duration = null);
    
    /// <summary>
    /// Updates cache after deleting an entity by ID
    /// </summary>
    /// <param name="id">The ID of the deleted entity</param>
    /// <param name="duration">Cache duration. Default is 10 minutes.</param>
    Task UpdateCacheAfterDeleteAsync(int id, TimeSpan? duration = null);

    /// <summary>
    /// Deletes an entity from cache by ID
    /// </summary>
    /// <param name="id">The ID of the entity to delete</param>
    Task DeleteEntityFromCacheAsync(int id);

    /// <summary>
    /// Deletes all entities from cache
    /// </summary>
    Task DeleteAllEntitiesFromCacheAsync();
}