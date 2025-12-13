namespace FuelMaster.Website.DTOs.Responses;

public class UserStatusResponse
{
    public bool IsAuthenticated { get; set; } = true;
    public bool HasActiveSubscription { get; set; }
    public bool HasActiveTenant { get; set; }
    public bool CanPerformOperations { get; set; }
    public UserInfo User { get; set; } = null!;
    public string? Message { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

