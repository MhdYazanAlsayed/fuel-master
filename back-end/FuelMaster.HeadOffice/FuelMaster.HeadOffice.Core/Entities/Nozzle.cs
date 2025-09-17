using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Nozzle : EntityBase<int>
    {
        public int TankId { get; set; }
        public Tank? Tank { get; set; }
        public int Number { get; set; }
        public NozzleStatus Status { get; set; }
        public string? ReaderNumber { get; set; }

        // Total amount sold via nozzle
        public decimal Amount { get; set; }

        // Total volume of fuel (in liters, for example) that was sold via nozzle
        public decimal Volume { get; set; }

        /*
            A cumulative (non-erasable) meter stores the total quantity used since the gun was first installed.
            This is a "kilometer counter" for the pump, for financial review and auditing.
        */
        public decimal Totalizer { get; set; }
        public int PumpId { get; set; }
        public Pump? Pump { get; set; }
        public decimal Price { get; set; }

        public Nozzle(int tankId , int pumpId, int number, NozzleStatus status, decimal amount, decimal volume, decimal totalizer, decimal price , string? readerNumber = null)
        {
            TankId = tankId;
            PumpId = pumpId;
            Number = number;
            Status = status;
            Amount = amount;
            Volume = volume;
            Totalizer = totalizer;
            ReaderNumber = readerNumber;
            Price = price;
        }
    }
}
