namespace FuelMaster.Website.DTOs.Responses;

public class UserResponse
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int TenantCount { get; set; }
    public int SubscriptionCount { get; set; }
}

