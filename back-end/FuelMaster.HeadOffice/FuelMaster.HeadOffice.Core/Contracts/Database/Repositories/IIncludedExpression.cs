using System.Linq.Expressions;

namespace FuelMaster.HeadOffice.Core.Contracts.Database.Repositories
{
    public interface IIncludedExpression<Type>: IBaseQueryableRepository<Type> where Type : class
    {
        IIncludedExpression<Type> Include(string navigationProps);
        IConditionExpression<Type> Where(Expression<Func<Type, bool>> expression);
    }
}
