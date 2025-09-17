using FuelMaster.HeadOffice.Core.Models.Dtos;
using System.Linq.Expressions;

namespace FuelMaster.HeadOffice.Core.Contracts.Database.Repositories
{
    public interface IBaseQueryableRepository<Type> where Type: class
    {
        Task<IReadOnlyList<Type>> ListAsync();
        Task<Type?> SingleOrDefaultAsync(Expression<Func<Type, bool>> expression);
        Task<Type> SingleAsync(Expression<Func<Type, bool>> expression);
        Task<Type?> FirstOrDefaultAsync(Expression<Func<Type, bool>> expression);
        Task<Type> FirstAsync(Expression<Func<Type, bool>> expression);
        Task<Type?> LastOrDefaultAsync(Expression<Func<Type, bool>> expression);
        Task<Type> LastAsync(Expression<Func<Type, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<Type, bool>> expression);
        Task<PaginationDto<Type>> PaginationAsync(int page);
    }
}
