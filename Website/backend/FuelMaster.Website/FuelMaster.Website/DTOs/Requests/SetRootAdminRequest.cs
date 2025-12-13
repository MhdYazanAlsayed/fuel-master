using System.ComponentModel.DataAnnotations;

namespace FuelMaster.Website.DTOs.Requests;

public class SetRootAdminRequest
{
    [Required]
    public bool IsRootAdmin { get; set; }
}

