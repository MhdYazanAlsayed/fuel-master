using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FuelMaster.HeadOffice.Core.Contracts.Database.Repositories
{
    public interface IRepository<Type> where Type : Base
    {
        DbSet<Type> Table { get; }
        IIncludedExpression<Type> Include(string prop);
        IConditionExpression<Type> Where(Expression<Func<Type, bool>> expression);
        Task CreateAsync(Type entity);
        void Update(Type entity);
        void Delete(Type entity);
    }
}
