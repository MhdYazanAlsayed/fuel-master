using System.ComponentModel.DataAnnotations;

namespace FuelMaster.Website.DTOs.Requests;

public class CreateTenantRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Tenant name can only contain lowercase letters, numbers, and hyphens")]
    public string Name { get; set; } = string.Empty;

}

