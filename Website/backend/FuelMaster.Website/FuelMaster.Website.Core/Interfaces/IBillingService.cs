using FuelMaster.Website.Core.Entities;

namespace FuelMaster.Website.Core.Interfaces;

public interface IBillingService
{
    Task<PaymentCard> AddPaymentCardAsync(string userId, PaymentCardInfo cardInfo);
    Task<IEnumerable<PaymentCard>> GetUserPaymentCardsAsync(string userId);
    Task<bool> DeletePaymentCardAsync(string userId, Guid cardId);
    Task<PaymentCard?> SetDefaultCardAsync(string userId, Guid cardId);
    Task<BillingHistory> ProcessBillingAsync(Guid subscriptionId);
    Task<IEnumerable<BillingHistory>> GetBillingHistoryAsync(string userId);
    Task<bool> RefundBillingAsync(Guid billingHistoryId);
}

