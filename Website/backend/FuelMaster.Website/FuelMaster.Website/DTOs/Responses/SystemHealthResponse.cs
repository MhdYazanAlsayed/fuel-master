namespace FuelMaster.Website.DTOs.Responses;

public class SystemHealthResponse
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int TotalTenants { get; set; }
    public int ActiveTenants { get; set; }
    public int TotalSubscriptions { get; set; }
    public int ActiveSubscriptions { get; set; }
    public DateTime LastChecked { get; set; }
}

