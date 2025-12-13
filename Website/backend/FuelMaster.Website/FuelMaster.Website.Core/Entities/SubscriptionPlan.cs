namespace FuelMaster.Website.Core.Entities;

public class SubscriptionPlan
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Enums.BillingCycle BillingCycle { get; set; }
    public bool IsFree { get; set; }
    public string? Features { get; set; } // JSON string for plan features
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
    public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
}

