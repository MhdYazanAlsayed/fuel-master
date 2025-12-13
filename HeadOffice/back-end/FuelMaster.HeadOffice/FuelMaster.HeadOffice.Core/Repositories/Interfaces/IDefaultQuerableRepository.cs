using System;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface IDefaultQuerableRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<(List<T>, int)> GetPaginationAsync(int currentPage, int pageSize);
}
