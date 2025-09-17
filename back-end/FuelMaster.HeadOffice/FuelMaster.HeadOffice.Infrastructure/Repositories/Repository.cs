using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Database.Repositories;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FuelMaster.HeadOffice.Infrastructure.Repositories
{
    public class Repository<Type> : IRepository<Type> where Type : Base
    {
        private readonly DbSet<Type> _table;
        public Repository(FuelMasterDbContext context)
        {
            _table = context.Set<Type>();
        }

        public DbSet<Type> Table => _table;

        public IIncludedExpression<Type> Include (string prop)
        {
            var result = new IncludedExpression<Type>(_table);

            result.Include(prop);

            return result;
        }

        public IConditionExpression<Type> Where (Expression<Func<Type, bool>> expression)
        {
            var result = new ConditionExpression<Type>(new List<string>(), _table);

            result.Where(expression);

            return result;
        }

        public async Task<Type?> SingleOrDefaultAsync (Expression<Func<Type , bool>> expression)
        {
            return await _table.SingleOrDefaultAsync(expression);
        }

        public async Task<Type> SingleAsync (Expression<Func<Type, bool>> expression)
        {
            return await _table.SingleAsync(expression);
        }

        public async Task<Type?> FirstOrDefaultAsync (Expression<Func<Type , bool>> expression)
        {
            return await _table.FirstOrDefaultAsync(expression);
        }

        public async Task<Type> FirstAsync(Expression<Func<Type, bool>> expression)
        {
            return await _table.FirstAsync(expression);
        }

        public async Task<Type?> LastOrDefaultAsync(Expression<Func<Type, bool>> expression)
        {
            return await _table.LastOrDefaultAsync(expression);
        }

        public async Task<Type> LastAsync(Expression<Func<Type, bool>> expression)
        {
            return await _table.LastAsync(expression);
        }

        public async Task<bool> AnyAsync (Expression<Func<Type, bool>> expression)
        {
            return await _table.AnyAsync(expression);
        }

        public async Task<PaginationDto<Type>> PaginationAsync (int page) 
        {
            return await _table.ToPaginationAsync(page);
        }



        public async Task CreateAsync(Type entity)
        {
            var entityInformationTable = entity as IInformationTable;

            if (entityInformationTable is null)
            {
                await _table.AddAsync(entity);
                return;
            }

            entityInformationTable.CreatedAt = DateTimeCulture.Now;

            await _table.AddAsync((Type)entityInformationTable);
        }

        public void Delete(Type entity)
        {
            _table.Remove(entity);
        }

        public void Update(Type entity)
        {
            _table.Update(entity);
        }

    }
}
