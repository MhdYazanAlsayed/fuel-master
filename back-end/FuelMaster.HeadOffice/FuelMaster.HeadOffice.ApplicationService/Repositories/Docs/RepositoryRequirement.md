# Repository requirement

**GOAL :** Create a class as a service that implements 'IXRepository'.
We have to use IEntityCache<T> in order to cache data in memory.
We have to cache all data at first, then in PaginationAsync method we will return chunk from cached data.
All get methods should return dto with CanDelete property. This property should check the related data with entity.

**Description :**

1. Create an interface for the repository and put basic operation inside it:
   - CreateAsync: returns ResultDto<DTO>.
   - EditAsync: returns ResultDto<DTO>.
   - DeleteAsync: retunrs ResultDto<DTO>.
   - DetailsAsync: returns DTO.
   - GetCached_X_Async: should get all data and cache them using IEntityCache<T> then return List<entity>.
   - GetAllAsync: retunrs List<DTO>. (will use GetCached_X_Async).
   - GetPaginationAsync: returns PaginationDto<DTO>. (will use GetCached_X_Async).
1. Implement that interface.
1. Create a controller or check if it exists already and do some refactor for it.
1. Use validator using FluentValidation for that controller.

**NOTE :**

1. Do not implement the code that check if there are related data to disable CanDelete property. Keep it a normal comment and I'll do it later on.
1. Do not cache the result of DetailsAsync, only get all data and search on them (The list won't be long.).
1. Create two folders in Domain.Contract (DTOs,Results) and put all types you need to use in the repository.
1. Use FuelMasterMapper.cs to map entity to result.
