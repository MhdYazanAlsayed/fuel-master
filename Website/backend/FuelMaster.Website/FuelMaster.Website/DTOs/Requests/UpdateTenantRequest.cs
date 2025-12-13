using System.ComponentModel.DataAnnotations;

namespace FuelMaster.Website.DTOs.Requests;

public class UpdateTenantRequest
{
    [StringLength(100, MinimumLength = 3)]
    [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Tenant name can only contain lowercase letters, numbers, and hyphens")]
    public string? Name { get; set; }

    public string? Status { get; set; } // "Active", "Suspended", "Deleted"
}

