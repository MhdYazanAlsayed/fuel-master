namespace FuelMaster.Website.Core.Interfaces;

public interface IPaymentProviderService
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    Task<bool> RefundPaymentAsync(string transactionId, decimal amount);
    Task<string> CreatePaymentTokenAsync(PaymentCardInfo cardInfo);
    Task<bool> ValidateCardAsync(PaymentCardInfo cardInfo);
}

public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string PaymentToken { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Dictionary<string, string>? Metadata { get; set; }
}

public class PaymentResult
{
    public bool Success { get; set; }
    public string? TransactionId { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, object>? ProviderResponse { get; set; }
}

public class PaymentCardInfo
{
    public string CardNumber { get; set; } = string.Empty;
    public string ExpiryMonth { get; set; } = string.Empty;
    public string ExpiryYear { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
    public string CardholderName { get; set; } = string.Empty;
}

