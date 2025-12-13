namespace FuelMaster.Website.Core.Entities;

public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? UserId { get; set; } // Nullable for system actions
    public string Action { get; set; } = string.Empty; // "CreateTenant", "UpdateSubscription", etc.
    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }
    public string? Changes { get; set; } // JSON string for Before/After values
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ApplicationUser? User { get; set; }
}

