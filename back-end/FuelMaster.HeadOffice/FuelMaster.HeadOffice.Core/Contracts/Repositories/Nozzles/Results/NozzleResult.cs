using FuelMaster.HeadOffice.Core.Contracts.Repositories.Tanks.Results;
using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Core.Contracts.Repositories.Nozzles.Results;

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
    // TODO: Implement CanDelete logic - check if NozzleHistory has any records referencing this nozzle
    public bool CanDelete { get; set; }
}

