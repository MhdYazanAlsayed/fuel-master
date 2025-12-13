using FuelMaster.Website.Core.Entities;

namespace FuelMaster.Website.Core.Interfaces;

public interface ISubscriptionPlanRepository : IRepository<SubscriptionPlan>
{
    Task<SubscriptionPlan?> GetByNameAsync(string name);
    Task<IEnumerable<SubscriptionPlan>> GetActivePlansAsync();
    Task<SubscriptionPlan?> GetFreePlanAsync();
}

