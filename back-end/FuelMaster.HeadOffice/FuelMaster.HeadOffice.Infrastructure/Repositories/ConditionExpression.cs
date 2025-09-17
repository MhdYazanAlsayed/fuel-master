using FuelMaster.HeadOffice.Core.Contracts.Database.Repositories;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories
{
    public class ConditionExpression<Type> : IConditionExpression<Type> where Type : class
    {
        private List<string> _navigationProps;
        private readonly DbSet<Type> _table;
        private List<Expression<Func<Type, bool>>> expressions;
        private IQueryableRepository<Type> _queryableRepository;
        public ConditionExpression(List<string> navigationProps , DbSet<Type> table)
        {
            _navigationProps = navigationProps;
            _table = table;
            expressions = new();
            _queryableRepository = new QueryableRepository<Type>();
        }

        public IConditionExpression<Type> Where (Expression<Func<Type, bool>> expression)
        {
            expressions.Add(expression);

            return this;
        }

        public async Task<IReadOnlyList<Type>> ListAsync ()
        {
            _queryableRepository.Query = ExecuteQuery();

            return await _queryableRepository.ListAsync();
        }

        public Task<Type?> SingleOrDefaultAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Type> SingleAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Type?> FirstOrDefaultAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Type> FirstAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Type?> LastOrDefaultAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Type> LastAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<Type, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationDto<Type>> PaginationAsync(int page)
        {
            throw new NotImplementedException();
        }

        private IQueryable<Type> ExecuteQuery ()
        {
            IQueryable<Type>? query = _table;

            foreach (var condition in expressions)
            {
                query = query.Where(condition);
            }

            foreach (var prop in _navigationProps)
            {
                query = query.Include(prop);
            }

            return query;
        }
    }
}
