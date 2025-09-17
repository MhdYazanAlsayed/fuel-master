using FuelMaster.HeadOffice.Core.Contracts.Database.Repositories;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using System.Collections;
using System.Linq.Expressions;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories
{
    public class QueryableRepository<Type> : IQueryableRepository<Type> where Type : class
    {
        public IQueryable<Type>? Query { get; set; } 

        public Task<bool> AnyAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Type> FirstAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Type?> FirstOrDefaultAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Type> LastAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Type?> LastOrDefaultAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Type>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PaginationDto<Type>> PaginationAsync(int page)
        {
            throw new NotImplementedException();
        }

        public Task<Type> SingleAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Type?> SingleOrDefaultAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
