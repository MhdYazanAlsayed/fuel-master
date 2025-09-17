using FuelMaster.HeadOffice.Core.Contracts.Database.Repositories;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories
{
    public class IncludedExpression<Type> : IIncludedExpression<Type> where Type : class
    {
        private List<string> _navigationProps = new List<string>();
        private readonly DbSet<Type> _table;
        private readonly IQueryableRepository<Type> _querableRepository;

        public IncludedExpression(DbSet<Type> table)
        {
            _table = table;
            _querableRepository = new QueryableRepository<Type>();
        }

        public IIncludedExpression<Type> Include (string navigationProps) 
        {
            _navigationProps.Add(navigationProps);

            return this;
        }

        public IConditionExpression<Type> Where (Expression<Func<Type, bool>> expression)
        {
            var result = new ConditionExpression<Type>(_navigationProps , _table);

            result.Where(expression);

            return result;
        }

        public async Task<IReadOnlyList<Type>> ListAsync ()
        {
            var query = ExecuteInclude();

            return await query.ToListAsync();
        }

        public async Task<Type?> SingleOrDefaultAsync(Expression<Func<Type, bool>> expression)
        {
            _querableRepository.Query = ExecuteInclude();

            return await _querableRepository.SingleOrDefaultAsync(expression);
        }

        public async Task<Type> SingleAsync(Expression<Func<Type, bool>> expression)
        {
            _querableRepository.Query = ExecuteInclude();

            return await _querableRepository.SingleAsync(expression);
        }

        public async Task<Type?> FirstOrDefaultAsync(Expression<Func<Type, bool>> expression)
        {
            _querableRepository.Query = ExecuteInclude();

            return await _querableRepository.FirstOrDefaultAsync(expression);
        }

        public async Task<Type> FirstAsync(Expression<Func<Type, bool>> expression)
        {
            _querableRepository.Query = ExecuteInclude();

            return await _querableRepository.FirstAsync(expression);
        }

        public async Task<Type?> LastOrDefaultAsync(Expression<Func<Type, bool>> expression)
        {
            _querableRepository.Query = ExecuteInclude();

            return await _querableRepository.LastOrDefaultAsync(expression);
        }

        public async Task<Type> LastAsync(Expression<Func<Type, bool>> expression)
        {
            _querableRepository.Query = ExecuteInclude();

            return await _querableRepository.LastAsync(expression);
        }

        public async Task<bool> AnyAsync(Expression<Func<Type, bool>> expression)
        {
            _querableRepository.Query = ExecuteInclude();

            return await _querableRepository.AnyAsync(expression);
        }

        public async Task<PaginationDto<Type>> PaginationAsync(int page)
        {
            _querableRepository.Query = ExecuteInclude();

            return await _querableRepository.PaginationAsync(page);
        }

        private IQueryable<Type> ExecuteInclude ()
        {
            IQueryable<Type> query = _table;

            foreach (var prop in _navigationProps)
            {
                query = query.Include(prop);
            }

            return query;
        }
    }
}
