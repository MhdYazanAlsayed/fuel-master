namespace FuelMaster.Website.Core.Entities;

public class BillingHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserSubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public Enums.BillingStatus Status { get; set; } = Enums.BillingStatus.Pending;
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public string? TransactionId { get; set; }
    public string? PaymentProviderResponse { get; set; } // JSON string
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public UserSubscription UserSubscription { get; set; } = null!;
}

