# Application Service Implementation Guide

**GOAL :**

Create a service that implements the provided interface. This service should implement the methods we want to expose using APIs:

1. CreateAsync
2. EditAsync (UpdateAsync)
3. DeleteAsync
4. GetAllAsync
5. GetPaginationAsync
6. DetailsAsync

## Implementation Requirements

### 1. Service Structure

- Implement the interface service (e.g., `IEntityService`)
- Use dependency injection for all dependencies
- Required dependencies:
  - `IEntityRepository` - Repository for the entity
  - `IMapper` - AutoMapper for entity/DTO mapping
  - `ISigninService` - Authorization service (for filtering by station/user)
  - `ILogger<TService>` - Logging service
  - `IEntityCache<TEntity>` - Cache service for the entity
  - `IUnitOfWork` - Unit of work for database transactions

### 2. Core Methods Implementation

#### GetCachedEntitiesAsync (Helper Method)

- Check cache first using `IEntityCache<TEntity>.GetAllEntitiesAsync()`
- If cache exists, apply authorization filtering if needed (e.g., filter by station ID)
- If cache doesn't exist:
  - Fetch from repository with appropriate includes (e.g., `includeStation: true, includeFuelType: true`)
  - Cache the entities using `IEntityCache<TEntity>.SetAllAsync(entities)`
  - Return the entities

#### GetAllAsync

- Use try-catch for error handling
- Get cached entities using `GetCachedEntitiesAsync()`
- Apply authorization filtering (get station ID from `ISigninService.TryToGetStationIdAsync()`)
- Apply any DTO-based filtering (e.g., filter by StationId from DTO)
- Map entities to result DTOs using `IMapper.Map<TResult>`
- Return `IEnumerable<TResult>`

#### GetPaginationAsync

- Use try-catch for error handling
- Call `GetAllAsync` to get all filtered entities
- Use `ToPagination(currentPage)` extension method to paginate
- Return `PaginationDto<TResult>`

#### CreateAsync

- Use try-catch for error handling
- Create new entity using entity constructor with DTO properties
- Call repository `Create(entity)`
- Save changes using `IUnitOfWork.SaveChangesAsync()`
- Map entity to result DTO
- Update cache incrementally: `IEntityCache<TEntity>.UpdateCacheAfterCreateAsync(entity)`
- Log success message
- Return `Result.Success(resultDto)`
- On exception: log error and return `Result.Failure<TResult>(Resource.EntityNotFound)`

#### EditAsync

- Use try-catch for error handling
- Get cached entities using `GetCachedEntitiesAsync()`
- Find entity by ID from cached data
- If not found, return `Result.Failure<TResult>(Resource.EntityNotFound)`
- Call entity's `Update(...)` method with DTO properties
- Call repository `Update(entity)`
- Save changes using `IUnitOfWork.SaveChangesAsync()`
- Map updated entity to result DTO
- Update cache incrementally: `IEntityCache<TEntity>.UpdateCacheAfterEditAsync(entity)`
- Log success message
- Return `Result.Success(updatedDto)`
- On exception: log error and return `Result.Failure<TResult>(Resource.EntityNotFound)`

#### DeleteAsync

- Use try-catch for error handling
- Get cached entities using `GetCachedEntitiesAsync()`
- Find entity by ID from cached data
- If not found, return `Result.Failure(Resource.EntityNotFound)`
- Call repository `Delete(entity)`
- Save changes using `IUnitOfWork.SaveChangesAsync()`
- Update cache incrementally: `IEntityCache<TEntity>.UpdateCacheAfterDeleteAsync(id)`
- Log success message
- Return `Result.Success()`
- On exception: log error and return `Result.Failure(Resource.EntityNotFound)`

#### DetailsAsync

- Use try-catch for error handling
- Get cached entities using `GetCachedEntitiesAsync()`
- Find entity by ID from cached data
- If not found, return `null`
- Map entity to result DTO
- Return `TResult?`

### 3. Best Practices

1. **Caching Strategy:**

   - Always check cache first before querying database
   - Cache entities, not DTOs
   - Update cache incrementally after create/update/delete operations
   - Do NOT cache details - only search in memory for details

2. **Error Handling:**

   - Wrap all methods in try-catch blocks
   - Log errors with appropriate context (entity ID, operation type, etc.)
   - Return appropriate error responses using `Result.Failure()`

3. **Mapping:**

   - Use AutoMapper (`IMapper`) to map between entities and DTOs
   - Map entities to result DTOs in GetAllAsync, GetPaginationAsync, and DetailsAsync

4. **Authorization:**

   - Apply authorization filtering in `GetCachedEntitiesAsync` if the entity has station/user relationships
   - Use `ISigninService.TryToGetStationIdAsync()` to get authorized station ID
   - Filter entities by station ID when authorization is required

5. **Logging:**

   - Log information messages for successful operations
   - Log error messages with full exception details
   - Include relevant context (IDs, counts, etc.) in log messages

6. **Code Organization:**
   - Use helper method `GetCachedEntitiesAsync()` to centralize cache logic
   - Follow consistent naming conventions
   - Keep methods focused and single-purpose

### 4. Example Service Structure

```csharp
public class EntityService : IEntityService
{
    private readonly IEntityRepository _repository;
    private readonly IMapper _mapper;
    private readonly ISigninService _authorization;
    private readonly ILogger<EntityService> _logger;
    private readonly IEntityCache<Entity> _entityCache;
    private readonly IUnitOfWork _unitOfWork;

    // Constructor with dependency injection

    public async Task<List<Entity>> GetCachedEntitiesAsync()
    {
        // Check cache, apply authorization, fetch from DB if needed
    }

    public async Task<IEnumerable<EntityResult>> GetAllAsync(GetEntityDto dto)
    {
        // Get cached entities, filter, map, return
    }

    public async Task<PaginationDto<EntityResult>> GetPaginationAsync(int currentPage, GetEntityDto dto)
    {
        // Use GetAllAsync and paginate
    }

    public async Task<ResultDto<EntityResult>> CreateAsync(EntityDto dto)
    {
        // Create, save, update cache, return result
    }

    public async Task<ResultDto<EntityResult>> EditAsync(int id, EntityDto dto)
    {
        // Get from cache, update, save, update cache, return result
    }

    public async Task<ResultDto> DeleteAsync(int id)
    {
        // Get from cache, delete, save, update cache, return result
    }

    public async Task<EntityResult?> DetailsAsync(int id)
    {
        // Get from cache, map, return
    }
}
```

### 5. Important Notes

- **Do NOT cache details** - only search in memory for details using cached entities
- **Do NOT check dependency entities** to mark CanDelete property (this is handled separately if needed)
- Always use cached data when available to avoid unnecessary database queries
- Update cache incrementally after create/update/delete operations to maintain consistency
- Use `IUnitOfWork.SaveChangesAsync()` to persist changes to database
- Return appropriate result types using `Result.Success()` and `Result.Failure()` helpers
