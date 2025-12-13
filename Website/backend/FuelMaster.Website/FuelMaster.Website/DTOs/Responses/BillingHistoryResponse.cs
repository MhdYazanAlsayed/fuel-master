namespace FuelMaster.Website.DTOs.Responses;

public class BillingHistoryResponse
{
    public Guid Id { get; set; }
    public Guid UserSubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public string? TransactionId { get; set; }
    public DateTime CreatedAt { get; set; }
}

