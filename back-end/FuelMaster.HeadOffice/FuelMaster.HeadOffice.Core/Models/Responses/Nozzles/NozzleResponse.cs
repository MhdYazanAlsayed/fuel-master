using FuelMaster.HeadOffice.Core.Enums;
using FuelMaster.HeadOffice.Core.Models.Responses.Pumps;
using FuelMaster.HeadOffice.Core.Models.Responses.Tanks;

namespace FuelMaster.HeadOffice.Core.Models.Responses.Nozzles
{
    public class NozzleResponse
    {
        public int Id { get; set; }
        public TankResponse? Tank { get; set; }
        public int Number { get; set; }
        public NozzleStatus Status { get; set; }
        public string? ReaderNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal Volume { get; set; }
        public decimal Totalizer { get; set; }
        public PumpResponse? Pump { get; set; }
    }
}
