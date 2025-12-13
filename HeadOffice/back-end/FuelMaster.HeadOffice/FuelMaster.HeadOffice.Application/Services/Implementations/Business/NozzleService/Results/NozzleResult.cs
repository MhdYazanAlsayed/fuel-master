using System;
using FuelMaster.HeadOffice.Core.Enums;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.TankService.Results;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService.Results;

public class NozzleResult
{
    public int Id { get; set; }
    public TankResult? Tank { get; set; }
    public int Number { get; set; }
    public NozzleStatus Status { get; set; }
    public string? ReaderNumber { get; set; }
    public decimal Amount { get; set; }
    public decimal Volume { get; set; }
    public decimal Totalizer { get; set; }
    public decimal Price { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool CanDelete { get; set; }
}

