namespace FuelMaster.Website.DTOs.Responses;

public class SubscriptionPlanResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string BillingCycle { get; set; } = string.Empty;
    public bool IsFree { get; set; }
    public string? Features { get; set; }
    public bool IsActive { get; set; }
}

