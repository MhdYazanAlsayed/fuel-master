
namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories;

public interface IRepository<T> where T : class 
{
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
    Task<T?> DetailsAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<(List<T>, int)> GetPaginationAsync(int page, int pageSize);
}