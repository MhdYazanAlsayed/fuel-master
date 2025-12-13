namespace FuelMaster.Website.Core.Entities;

public class PaymentCard
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = string.Empty;
    public string CardLastFour { get; set; } = string.Empty; // Last 4 digits
    public string CardBrand { get; set; } = string.Empty; // Visa, Mastercard, etc.
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public bool IsDefault { get; set; }
    public string Token { get; set; } = string.Empty; // Encrypted payment token from provider
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ApplicationUser User { get; set; } = null!;
}

