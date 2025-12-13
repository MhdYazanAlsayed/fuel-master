using FuelMaster.Website.Core.Entities;

namespace FuelMaster.Website.Core.Interfaces;

public interface IPaymentCardRepository : IRepository<PaymentCard>
{
    Task<IEnumerable<PaymentCard>> GetCardsByUserIdAsync(string userId);
    Task<PaymentCard?> GetDefaultCardByUserIdAsync(string userId);
    Task SetDefaultCardAsync(string userId, Guid cardId);
}

