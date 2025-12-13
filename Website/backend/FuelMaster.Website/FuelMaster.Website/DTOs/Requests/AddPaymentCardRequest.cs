using System.ComponentModel.DataAnnotations;

namespace FuelMaster.Website.DTOs.Requests;

public class AddPaymentCardRequest
{
    [Required]
    [CreditCard]
    public string CardNumber { get; set; } = string.Empty;

    [Required]
    [Range(1, 12)]
    public int ExpiryMonth { get; set; }

    [Required]
    [Range(2024, 2099)]
    public int ExpiryYear { get; set; }

    [Required]
    [StringLength(4, MinimumLength = 3)]
    public string Cvv { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string CardholderName { get; set; } = string.Empty;
}

