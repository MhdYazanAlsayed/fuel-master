namespace FuelMaster.Website.DTOs.Responses;

public class PaymentCardResponse
{
    public Guid Id { get; set; }
    public string CardLastFour { get; set; } = string.Empty;
    public string CardBrand { get; set; } = string.Empty;
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }
}

