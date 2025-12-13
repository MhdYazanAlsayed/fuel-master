# Repository Implementation Guide

**GOAL:**

Create a repository that implements the repository interface. The repository should provide data access methods for CRUD operations and querying entities.

## Repository Interface Decision

### When to Use `IDefaultQuerableRepository<T>`

**Use `IDefaultQuerableRepository<T>` when you DON'T need to include related entities (no navigation properties).**

**Interface Structure:**

```csharp
public interface IEntityRepository : IDefaultQuerableRepository<Entity>, IRepository<Entity>, IScopedDependency
{
    // No need to define GetAllAsync or GetPaginationAsync - they come from IDefaultQuerableRepository
    // Optional: Add DetailsAsync if needed
    Task<Entity?> DetailsAsync(int id);
}
```

**Example:** `ICityRepository`, `IFuelTypeRepository`

### When to Define Methods Directly in Interface

**Define methods directly in the repository interface when you NEED to include related entities (navigation properties).**

**Interface Structure:**

```csharp
public interface IEntityRepository : IRepository<Entity>, IScopedDependency
{
    // Define methods with include parameters
    Task<List<Entity>> GetAllAsync(bool includeRelated1 = false, bool includeRelated2 = false);
    Task<(List<Entity>, int)> GetPaginationAsync(int page, int pageSize, bool includeRelated1 = false, bool includeRelated2 = false);
    // Optional: Add DetailsAsync if needed
    Task<Entity?> DetailsAsync(int id);
}
```

**Example:** `ITankRepository`, `IStationRepository`

## Implementation Requirements

### 1. Repository Structure

- Implement the repository interface (e.g., `IEntityRepository`)
- Use dependency injection for `IContextFactory<FuelMasterDbContext>`
- Access database context via `contextFactory.CurrentContext`
- Store context in private readonly field: `_context`

### 2. Required Methods

All repositories must implement these methods from `IRepository<T>`:

#### Create

```csharp
public Entity Create(Entity entity)
{
    _context.Entities.Add(entity);
    return entity;
}
```

#### Update

```csharp
public Entity Update(Entity entity)
{
    _context.Entities.Update(entity);
    return entity;
}
```

#### Delete

```csharp
public Entity Delete(Entity entity)
{
    _context.Entities.Remove(entity);
    return entity;
}
```

### 3. Query Methods Implementation

#### GetAllAsync (No Includes - Using IDefaultQuerableRepository)

**When using `IDefaultQuerableRepository<T>`:**

```csharp
public async Task<List<Entity>> GetAllAsync()
{
    return await _context.Entities.ToListAsync();
}
```

#### GetAllAsync (With Includes - Custom Interface)

**When defining methods directly in interface:**

```csharp
public async Task<List<Entity>> GetAllAsync(bool includeRelated1 = false, bool includeRelated2 = false)
{
    var query = _context.Entities.AsQueryable();

    if (includeRelated1)
    {
        query = query.Include(e => e.RelatedEntity1);
    }

    if (includeRelated2)
    {
        query = query.Include(e => e.RelatedEntity2);
    }

    return await query.ToListAsync();
}
```

**Pattern:**

1. Start with `_context.Entities.AsQueryable()`
2. Conditionally add `.Include()` for each related entity parameter
3. Return `await query.ToListAsync()`

#### GetPaginationAsync (No Includes - Using IDefaultQuerableRepository)

**When using `IDefaultQuerableRepository<T>`:**

```csharp
public async Task<(List<Entity>, int)> GetPaginationAsync(int currentPage, int pageSize)
{
    var count = await _context.Entities.CountAsync();
    var data = await _context.Entities
        .Skip((currentPage - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    return (data, count);
}
```

**Note:** Parameter names should be `currentPage` and `pageSize` to match `IDefaultQuerableRepository` interface.

#### GetPaginationAsync (With Includes - Custom Interface)

**When defining methods directly in interface:**

```csharp
public async Task<(List<Entity>, int)> GetPaginationAsync(int page, int pageSize, bool includeRelated1 = false, bool includeRelated2 = false)
{
    var query = _context.Entities.AsQueryable();

    if (includeRelated1)
    {
        query = query.Include(e => e.RelatedEntity1);
    }

    if (includeRelated2)
    {
        query = query.Include(e => e.RelatedEntity2);
    }

    var count = await query.CountAsync();
    var data = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return (data, count);
}
```

**Pattern:**

1. Start with `_context.Entities.AsQueryable()`
2. Conditionally add `.Include()` for each related entity parameter
3. Get count: `await query.CountAsync()`
4. Apply pagination: `.Skip((page - 1) * pageSize).Take(pageSize)`
5. Return tuple: `(data, count)`

**Note:** Parameter name should be `page` (not `currentPage`) when defining custom interface methods.

#### DetailsAsync (Optional)

```csharp
public async Task<Entity?> DetailsAsync(int id)
{
    return await _context.Entities.FindAsync(id);
}
```

## Complete Examples

### Example 1: Repository with No Includes (Using IDefaultQuerableRepository)

**Interface:**

```csharp
namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface ICityRepository : IDefaultQuerableRepository<City>, IRepository<City>, IScopedDependency
{
    Task<City?> DetailsAsync(int id);
}
```

**Implementation:**

```csharp
using FuelMaster.HeadOffice.Core.Interfaces.Database;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories;

public class CityRepository : ICityRepository
{
    private readonly FuelMasterDbContext _context;

    public CityRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }

    public City Create(City entity)
    {
        _context.Cities.Add(entity);
        return entity;
    }

    public City Delete(City entity)
    {
        _context.Cities.Remove(entity);
        return entity;
    }

    public async Task<City?> DetailsAsync(int id)
    {
        return await _context.Cities.FindAsync(id);
    }

    public async Task<List<City>> GetAllAsync()
    {
        return await _context.Cities.ToListAsync();
    }

    public async Task<(List<City>, int)> GetPaginationAsync(int currentPage, int pageSize)
    {
        var count = await _context.Cities.CountAsync();
        var data = await _context.Cities
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (data, count);
    }

    public City Update(City entity)
    {
        _context.Cities.Update(entity);
        return entity;
    }
}
```

### Example 2: Repository with Includes (Custom Interface Methods)

**Interface:**

```csharp
namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface ITankRepository : IRepository<Tank>, IScopedDependency
{
    Task<List<Tank>> GetAllAsync(bool includeStation = false, bool includeFuelType = false, bool includeNozzles = false);
    Task<(List<Tank>, int)> GetPaginationAsync(int page, int pageSize, bool includeStation = false, bool includeFuelType = false, bool includeNozzles = false);
}
```

**Implementation:**

```csharp
using System;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Database;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories;

public class TankRepository : ITankRepository
{
    private readonly FuelMasterDbContext _context;

    public TankRepository(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _context = contextFactory.CurrentContext;
    }

    public Tank Create(Tank entity)
    {
        _context.Tanks.Add(entity);
        return entity;
    }

    public Tank Delete(Tank entity)
    {
        _context.Tanks.Remove(entity);
        return entity;
    }

    public Task<List<Tank>> GetAllAsync(bool includeStation = false, bool includeFuelType = false, bool includeNozzles = false)
    {
        var query = _context.Tanks.AsQueryable();

        if (includeStation)
        {
            query = query.Include(t => t.Station);
        }

        if (includeFuelType)
        {
            query = query.Include(t => t.FuelType);
        }

        if (includeNozzles)
        {
            query = query.Include(t => t.Nozzles);
        }

        return query.ToListAsync();
    }

    public async Task<(List<Tank>, int)> GetPaginationAsync(int page, int pageSize, bool includeStation = false, bool includeFuelType = false, bool includeNozzles = false)
    {
        var query = _context.Tanks.AsQueryable();

        if (includeStation)
        {
            query = query.Include(t => t.Station);
        }

        if (includeFuelType)
        {
            query = query.Include(t => t.FuelType);
        }

        if (includeNozzles)
        {
            query = query.Include(t => t.Nozzles);
        }

        var count = await query.CountAsync();
        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, count);
    }

    public Tank Update(Tank entity)
    {
        _context.Tanks.Update(entity);
        return entity;
    }
}
```

## Best Practices

### 1. Naming Conventions

- Repository class: `{EntityName}Repository`
- Interface: `I{EntityName}Repository`
- Context property: `_context` (private readonly)
- DbSet access: `_context.{EntityPluralName}` (e.g., `_context.Cities`, `_context.Tanks`)

### 2. Include Parameters

- Use descriptive boolean parameter names: `includeStation`, `includeFuelType`, `includeNozzles`
- Default all include parameters to `false`
- Use navigation property names in `.Include()` calls

### 3. Pagination

- **IDefaultQuerableRepository**: Use `currentPage` parameter name
- **Custom interface**: Use `page` parameter name
- Always calculate count before applying pagination
- Return tuple: `(List<Entity>, int)` where int is the total count

### 4. Query Building

- Always start with `AsQueryable()` when using includes
- Build query conditionally before executing
- Use `ToListAsync()` for async execution

### 5. Required Usings

```csharp
using FuelMaster.HeadOffice.Core.Interfaces.Database;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
```

### 6. Interface Inheritance

- Always extend `IRepository<T>` for CRUD operations
- Extend `IDefaultQuerableRepository<T>` only when no includes needed
- Add `IScopedDependency` marker interface for dependency injection
- Define custom methods directly in interface when includes are needed

## Decision Flowchart

```
Do you need to include related entities?
│
├─ NO → Use IDefaultQuerableRepository<T>
│        ├─ Interface: extends IDefaultQuerableRepository<T>, IRepository<T>, IScopedDependency
│        ├─ GetAllAsync(): no parameters, simple ToListAsync()
│        └─ GetPaginationAsync(currentPage, pageSize): no include parameters
│
└─ YES → Define methods directly in interface
          ├─ Interface: extends IRepository<T>, IScopedDependency
          ├─ GetAllAsync(bool includeX = false, ...): with include parameters
          └─ GetPaginationAsync(page, pageSize, bool includeX = false, ...): with include parameters
```

## Important Notes

- **Always implement** `Create`, `Update`, and `Delete` from `IRepository<T>`
- **Use `IDefaultQuerableRepository<T>`** when you don't need any includes - it provides default implementations
- **Define methods directly** in your interface when you need includes - you must provide the implementation
- **Parameter naming**: `currentPage` for IDefaultQuerableRepository, `page` for custom interfaces
- **Count before pagination**: Always get count before applying Skip/Take
- **Return tuples**: GetPaginationAsync returns `(List<Entity>, int)` where int is total count
- **Optional DetailsAsync**: Add if you need to fetch a single entity by ID
