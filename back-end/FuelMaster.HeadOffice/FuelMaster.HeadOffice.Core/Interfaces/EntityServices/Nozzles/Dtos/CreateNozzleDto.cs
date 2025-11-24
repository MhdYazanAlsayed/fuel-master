using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories.Nozzles.Dtos;

public class CreateNozzleDto
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

