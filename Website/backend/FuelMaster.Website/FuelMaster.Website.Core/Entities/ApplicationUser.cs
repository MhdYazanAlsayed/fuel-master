using Microsoft.AspNetCore.Identity;

namespace FuelMaster.Website.Core.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<UserSubscription> Subscriptions { get; set; } = new List<UserSubscription>();
    public ICollection<PaymentCard> PaymentCards { get; set; } = new List<PaymentCard>();
    public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
}

