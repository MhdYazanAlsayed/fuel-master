using System;
using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.NozzleService.DTOs;

public class NozzleDto
{
    public int TankId { get; set; }
    public int PumpId { get; set; }
    public int Number { get; set; }
    public NozzleStatus Status { get; set; }
    public string? ReaderNumber { get; set; }
    public decimal Amount { get; set; }
    public decimal Volume { get; set; }
    public decimal Totalizer { get; set; }
}

