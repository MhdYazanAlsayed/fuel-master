# FuelMaster HeadOffice - Business Rules Documentation

This document outlines the core business rules and constraints that govern the FuelMaster HeadOffice system. These rules define how entities can be created, modified, and deleted, as well as the relationships and dependencies between different system components.

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Cities](#cities)
3. [Fuel Types](#fuel-types)
4. [General Business Rules](#general-business-rules)

---

## Architecture Overview

### Generic Cache Service

The system uses a **generic cache service** (`IEntityCache<T>`) to share cached entity data across repositories and services. This architecture provides significant performance benefits by avoiding unnecessary database queries.

#### Cache Service Architecture

- **Generic Interface**: `IEntityCache<T>` - Generic cache interface for any entity type
- **Generic Implementation**: `EntityCache<T>` - Reusable cache implementation
- **Cache Key Generation**: Uses `nameof(T)` to generate type-safe cache keys (e.g., `"Station_All"`, `"City_All"`)
- **Entity-Based Caching**: Caches **entities** (domain models) instead of DTOs, allowing flexible mapping to different result types

#### Benefits

- **Performance**: Reduces database queries by caching frequently accessed entities
- **Reusability**: Generic cache service works for all entity types (Station, City, Tank, etc.)
- **Consistency**: Ensures all services access the same cached data
- **Flexibility**: Entities can be mapped to different DTOs as needed without re-caching

#### How It Works

1. **Cache Storage**: Entities are cached after initial database fetch
2. **Cache Retrieval**: Repositories check cache first before querying database
3. **Cache Updates**: Cache is automatically updated when entities are created, updated, or deleted
4. **Cross-Service Access**: Different repositories can access the same cached entities

**Example**: When checking if a city can be deleted, the `CityRepository` uses `IEntityCache<Station>` to get cached stations instead of querying the database directly, significantly improving performance.

---

## Cities

### Overview

Cities represent geographical locations where fuel stations operate. Each city is associated with a zone and can have multiple stations.

### Entity Structure

**Database Entity:**

- **Id**: Unique identifier
- **ArabicName**: City name in Arabic (required)
- **EnglishName**: City name in English (required)

**Response DTO (CityResult):**

- **Id**: Unique identifier
- **ArabicName**: City name in Arabic
- **EnglishName**: City name in English
- **CanDelete**: Boolean flag indicating if the city can be deleted (computed property - **NOT stored in database**)

### Business Rules

#### 1. Creation

- **Who can create**:
  - Administrators
  - Users with appropriate permissions
- **Requirements**:
  - `ArabicName` must be provided and cannot be null or empty
  - `EnglishName` must be provided and cannot be null or empty
- **Validation**:
  - Both names must be unique (system should validate for duplicates)
  - Names cannot exceed maximum length restrictions

#### 2. Update

- **Who can update**:
  - Administrators
  - Users with appropriate permissions
- **Allowed modifications**:
  - `ArabicName` can be updated
  - `EnglishName` can be updated
- **Requirements**:
  - Updated names must still meet the same validation rules as creation
  - Cannot update to duplicate names of existing cities
- **Restrictions**:
  - `Id` cannot be modified (immutable)

#### 3. Deletion

- **Who can delete**:

  - Administrators
  - Users with appropriate permissions **AND** `CanDelete` must be `true`

- **CanDelete Property**:
  - **Purpose**: Indicates whether a city can be deleted based on data dependencies
  - **Location**: Property in `CityResult` DTO - **NOT a database column**
  - **Calculation Method**: **Computed dynamically** using cached entity data (no direct database query)
  - **Logic**:
    - `CanDelete = true`: City has no dependencies and can be safely deleted
    - `CanDelete = false`: City is referenced by other entities and **cannot** be deleted
- **Dependency Checking**:
  The system checks for dependencies using **cached entity data** instead of querying the database:
  - **Stations Check**: Uses `IEntityCache<Station>` to retrieve cached stations
    - Checks if any cached station references this city: `stations.Any(s => s.CityId == cityId)`
    - If any station references the city → `CanDelete = false`
    - If no stations reference the city → `CanDelete = true`
  - **Performance Benefit**: Uses cached data instead of executing database queries
  - **Other Future Dependencies**: Any other entity that references the city should be checked similarly using cached data
- **Calculation Logic**:

  ```csharp
  // Get cached stations (no database query)
  var stations = await _stationCache.GetAllEntitiesAsync();

  // Calculate CanDelete using cached data
  CanDelete = stations == null || !stations.Any(s => s.CityId == city.Id)

  // Logic:
  // - If no cached stations exist, CanDelete = true
  // - If no stations reference this city, CanDelete = true
  // - If any station references this city, CanDelete = false
  ```

- **Deletion Rules**:
  - The `CanDelete` property is calculated before deletion is attempted
  - Delete endpoint should query dependencies and set `CanDelete` accordingly
  - If `CanDelete = false`, the system will **block** the deletion operation before attempting database deletion
  - Database foreign key constraints (DeleteBehavior.Restrict) provide a secondary safeguard
  - Users should receive a clear error message explaining why the city cannot be deleted

#### 4. Data Integrity

- **Foreign Key Relationships**:
  - `Stations.CityId` → `Cities.Id` (Restrict on delete)
  - When a city is referenced by a station, deletion is prevented at the database level
- **Referential Integrity**:

  - Cities cannot be deleted if they have associated stations
  - The `CanDelete` property is computed using **cached station entities** (via `IEntityCache<Station>`) instead of querying the database:

    ```csharp
    // Get stations from cache (no database query)
    var stations = await _stationCache.GetAllEntitiesAsync();

    // Check if city is referenced by any station
    CanDelete = stations == null || !stations.Any(s => s.CityId == city.Id)
    ```

  - The check is performed:
    - **When retrieving city details** (GET /api/cities/{id})
    - **When retrieving city list** (GET /api/cities)
    - **When retrieving paginated cities** (GET /api/cities/pagination)
    - **Before attempting deletion** (DELETE /api/cities/{id})
  - **Performance Optimization**: Uses cached entity data instead of database queries, significantly improving response times
  - **Cache Invalidation**: Cache is automatically updated when stations are created, updated, or deleted, ensuring `CanDelete` values stay accurate

#### 5. API Behavior

- **GET /api/cities**:
  - Returns list of cities with `CanDelete` property
  - Each city's `CanDelete` is calculated using cached stations (no database query)
  - Uses `IEntityCache<Station>` to retrieve cached station entities
- **GET /api/cities/pagination**:
  - Returns paginated list of cities with `CanDelete` property
  - Retrieves cached cities and cached stations in parallel
  - Calculates `CanDelete` for each city using cached station data
  - Example calculation:
    ```csharp
    var stations = await _stationCache.GetAllEntitiesAsync();
    CanDelete = stations == null || !stations.Any(s => s.CityId == city.Id)
    ```
- **GET /api/cities/{id}**:
  - Returns city details with `CanDelete` property
  - Calculates `CanDelete` using cached stations: checks if any cached station references this city
  - If no stations reference the city, `CanDelete = true`; otherwise `CanDelete = false`
  - Example response:
    ```json
    {
      "id": 1,
      "arabicName": "عمان",
      "englishName": "Amman",
      "canDelete": false
    }
    ```
- **DELETE /api/cities/{id}**:
  - Validates user permissions
  - **Checks dependencies** using cached stations to calculate `CanDelete`:
    - Retrieves cached stations via `IEntityCache<Station>`
    - Checks if any station references this city
    - If dependencies exist, `CanDelete = false`
  - If `CanDelete = false`, returns error without attempting deletion
  - If `CanDelete = true`, attempts database deletion
  - Database foreign key constraints provide secondary protection

#### 6. Implementation Notes

- **Cache-Based Calculation**:

  - `CanDelete` is **NOT stored in the database**
  - It is a **property in the `CityResult` DTO**
  - It is calculated at runtime using **cached entity data** (not database queries)
  - Each API response calculates the value using cached stations

- **Cache Service Usage**:

  - Repository uses `IEntityCache<Station>` to retrieve cached station entities
  - No database queries are executed to check dependencies
  - Performance is significantly improved by using cached data

- **Implementation Example**:

  ```csharp
  // In CityRepository
  private readonly IEntityCache<City> _cityCache;
  private readonly IEntityCache<Station> _stationCache;

  public async Task<PaginationDto<CityResult>> GetPaginationAsync(int currentPage)
  {
      var allCities = await GetAllAsync(); // Gets from _cityCache
      var stations = await _stationCache.GetAllEntitiesAsync(); // Gets cached stations

      var pageData = allCities
          .Skip(...)
          .Take(...)
          .Select(c => new CityResult
          {
              Id = c.Id,
              ArabicName = c.ArabicName,
              EnglishName = c.EnglishName,
              // Calculate CanDelete using cached stations (no DB query)
              CanDelete = stations == null || !stations.Any(s => s.CityId == c.Id)
          })
          .ToList();
  }
  ```

- **Response Mapping**:

  - When mapping `City` entity to `CityResult` DTO:
    - Retrieve cached stations using `IEntityCache<Station>`
    - Calculate `CanDelete` based on cached station data
    - Include `CanDelete` in the response object
    - This happens in the repository layer

- **Performance Optimization**:

  - **Cache-First Approach**: Uses cached entities instead of database queries
  - **Batch Operations**: Single cache retrieval for all stations, then checks for all cities
  - **Automatic Cache Updates**: Cache is automatically updated when stations are created/updated/deleted
  - **No Additional Caching Needed**: Dependency checks use already-cached data

- **Code Location**:

  - Dependency check logic is in the repository layer
  - Uses generic cache services (`IEntityCache<T>`) for cross-entity data access
  - Controller should not contain dependency checking logic (separation of concerns)

- **Cache Invalidation**:
  - When a station is created: `IEntityCache<Station>` is automatically updated
  - When a station is updated: Cache is refreshed automatically
  - When a station is deleted: Cache is updated, affecting `CanDelete` for relevant cities
  - All cache operations are handled by the generic `EntityCache<T>` implementation

---

## Fuel Types

### Overview

Fuel types define the different categories of fuel available in the system (e.g., Gasoline 95, Diesel, Premium). Each tank stores one fuel type, and transactions record which fuel type was sold.

### Entity Structure

- **Id**: Unique identifier
- **ArabicName**: Fuel type name in Arabic (required)
- **EnglishName**: Fuel type name in English (required)

### Business Rules

#### 1. Creation

- **Who can create**:
  - Administrators
  - Users with appropriate permissions
- **Requirements**:
  - `ArabicName` must be provided and cannot be null or empty
  - `EnglishName` must be provided and cannot be null or empty
- **Validation**:
  - Both names must be unique (system should validate for duplicates)
  - Names cannot exceed maximum length restrictions

#### 2. Update

- **Who can update**:
  - Administrators
  - Users with appropriate permissions
- **Allowed modifications**:
  - `ArabicName` can be updated
  - `EnglishName` can be updated
- **Requirements**:
  - Updated names must still meet the same validation rules as creation
  - Cannot update to duplicate names of existing fuel types
- **Restrictions**:
  - `Id` cannot be modified (immutable)
  - Updates are allowed even if the fuel type has dependencies (e.g., tanks using this fuel type)

#### 3. Deletion

- **System Policy**: **NO DELETE FUNCTIONALITY** exists for fuel types in the system
- **Rule**:
  - Fuel types **cannot be deleted** from the system
  - There is no DELETE endpoint for fuel types
  - Even if a fuel type has no dependencies, deletion is not supported
- **Rationale**:
  - Fuel types are considered fundamental reference data
  - Historical transactions reference fuel types
  - Tanks are associated with fuel types
  - Deleting fuel types would break data integrity and historical records

#### 4. Dependencies

- **Entities that reference Fuel Types**:
  - **Tanks**: Each tank has a `FuelTypeId` foreign key (DeleteBehavior.Restrict)
  - **Transactions**: Historical transactions may reference fuel types through tank relationships
  - **Zone Prices**: Pricing may be set per fuel type
- **Data Integrity**:
  - Foreign key constraints prevent accidental deletion even if delete functionality existed
  - All relationships use `DeleteBehavior.Restrict` to maintain referential integrity

#### 5. Soft Delete Consideration

- While hard deletion is not supported, the system may consider:
  - Marking fuel types as "inactive" or "archived" (future enhancement)
  - Filtering inactive fuel types from active selection lists
  - Maintaining historical data integrity

#### 6. API Behavior

- **GET /api/fuel-types**: Returns all fuel types (no deletion flags)
- **POST /api/fuel-types**: Creates new fuel type
- **PUT /api/fuel-types/{id}**: Updates existing fuel type
- **DELETE /api/fuel-types/{id}**: **DOES NOT EXIST** - Not implemented

#### 7. Implementation Notes

- Repository should not expose delete methods for fuel types
- Controllers should not have delete endpoints
- Documentation should clearly state that deletion is not supported
- If a fuel type is no longer needed, it can remain in the system but may be hidden from active selection lists (if such functionality is added)

---

## General Business Rules

### Permission-Based Access

- All create, update, and delete operations require appropriate user permissions
- Administrators have full access to all operations
- Regular users must have specific permissions granted through the group/permission system

### Data Dependencies

- The system enforces data integrity through:
  - Foreign key constraints at the database level
  - Business logic checks at the application level
  - Validation rules before operations

### Naming Conventions

- All entities with names must support both Arabic and English
- Both language names are required and cannot be null or empty
- Names should be unique within their entity type

### Audit Trail

- All entities inherit from base classes that track:
  - `CreatedAt`: When the entity was created
  - `UpdatedAt`: When the entity was last updated (if applicable)

### Multi-Tenancy

- All business rules apply within the context of a tenant
- Data isolation is maintained at the database level
- Each tenant has independent cities, fuel types, and other entities

### Generic Cache Service

#### Overview

The system implements a **generic cache service** (`IEntityCache<T>`) that provides entity caching across all repositories and services. This service caches domain entities instead of DTOs, allowing for flexible data access and mapping.

#### Architecture

- **Generic Interface**: `IEntityCache<T>` where T is the entity type
- **Generic Implementation**: `EntityCache<T>` - Reusable for all entity types
- **Cache Key Strategy**: Uses `nameof(T)` to generate type-safe cache keys
  - Example: `nameof(Station)` → `"Station_All"` cache key
  - Example: `nameof(City)` → `"City_All"` cache key

#### Benefits

- **Performance**: Reduces database queries by caching frequently accessed entities
- **Reusability**: Single generic implementation works for all entities (Station, City, Tank, etc.)
- **Consistency**: All services access the same cached data
- **Flexibility**: Entities can be mapped to different DTOs as needed

#### Usage Pattern

```csharp
// Repository injects entity cache
private readonly IEntityCache<City> _cityCache;
private readonly IEntityCache<Station> _stationCache;

// Get entities from cache (no database query if cached)
var cities = await _cityCache.GetAllEntitiesAsync();
var stations = await _stationCache.GetAllEntitiesAsync();

// Cache entities after database fetch
await _cityCache.SetAllAsync(cities);

// Update cache on entity changes
await _cityCache.UpdateCacheAfterCreateAsync(newCity);
await _cityCache.UpdateCacheAfterEditAsync(updatedCity);
await _cityCache.UpdateCacheAfterDeleteAsync(id);
```

#### Cross-Service Data Access

- Repositories can access cached entities from other services
- Example: `CityRepository` uses `IEntityCache<Station>` to check city dependencies
- No database queries needed when checking dependencies - uses cached data
- Significantly improves performance for dependency checks

---

## Future Considerations

### Cities

- Consider implementing soft delete functionality
- Add cascade delete options for administrators (with warnings)
- Implement bulk operations for city management

### Fuel Types

- Consider adding an "Active/Inactive" status field
- Implement archival mechanism for historical fuel types
- Add fuel type categories or groups

---

**Last Updated**: [Date]  
**Version**: 1.0  
**Document Status**: Active
