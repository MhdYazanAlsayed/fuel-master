using System.Linq.Expressions;

namespace FuelMaster.HeadOffice.Core.Contracts.Database.Repositories
{
    public interface IConditionExpression<Type> : IBaseQueryableRepository<Type> where Type : class
    {
        IConditionExpression<Type> Where(Expression<Func<Type, bool>> expression);
    }
}
