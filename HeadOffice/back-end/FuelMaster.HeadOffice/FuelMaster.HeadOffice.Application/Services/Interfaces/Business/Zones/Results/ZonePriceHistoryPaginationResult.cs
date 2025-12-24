using System;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Zones.Results;

public class ZonePriceHistoryPaginationResult
{
    public FuelType FuelType { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string EmployeeName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public decimal PriceBeforeChange { get; set; }
    public decimal PriceAfterChange { get; set; }
}