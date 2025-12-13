namespace FuelMaster.Website.DTOs.Responses;

public class UserSubscriptionResponse
{
    public Guid Id { get; set; }
    public Guid PlanId { get; set; }
    public SubscriptionPlanResponse Plan { get; set; } = null!;
    public string Status { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

