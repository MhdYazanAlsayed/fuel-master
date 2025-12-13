namespace FuelMaster.Website.Core.Entities;

public class Tenant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty; // Unique, e.g., "fuel-star"
    public string UserId { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public Enums.TenantStatus Status { get; set; } = Enums.TenantStatus.Active;
    public Guid PlanId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    // Navigation properties
    public ApplicationUser User { get; set; } = null!;
    public SubscriptionPlan Plan { get; set; } = null!;
    public ICollection<TenantBackup> Backups { get; set; } = new List<TenantBackup>();
}

