namespace FuelMaster.Website.Core.Entities;

public class UserSubscription
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = string.Empty;
    public Guid PlanId { get; set; }
    public Enums.SubscriptionStatus Status { get; set; } = Enums.SubscriptionStatus.Active;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ApplicationUser User { get; set; } = null!;
    public SubscriptionPlan Plan { get; set; } = null!;
    public ICollection<BillingHistory> BillingHistories { get; set; } = new List<BillingHistory>();
}

