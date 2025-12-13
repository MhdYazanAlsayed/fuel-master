using System.ComponentModel.DataAnnotations;

namespace FuelMaster.Website.DTOs.Requests;

public class SubscribeRequest
{
    [Required]
    public Guid PlanId { get; set; }
}

