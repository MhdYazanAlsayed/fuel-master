using FuelMaster.HeadOffice.Core.Enums;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Nozzle : Entity<int>
    {
        public int TankId { get; private set; }
        public Tank? Tank { get; private set; }
        public int Number { get; private set; }
        public NozzleStatus Status { get; private set; }
        public string? ReaderNumber { get; private set; }

        // Total amount sold via nozzle
        public decimal Amount { get; private set; }

        // Total volume of fuel (in liters, for example) that was sold via nozzle
        public decimal Volume { get; private set; }

        /*
            A cumulative (non-erasable) meter stores the total quantity used since the gun was first installed.
            This is a "kilometer counter" for the pump, for financial review and auditing.
        */
        public decimal Totalizer { get; private set; }
        public int PumpId { get; private set; }
        public Pump? Pump { get; private set; }

        /*
            Price does not accessible from this class, It's only set by the zone.
            To avoid multiple includes in the query.
        */
        public decimal Price { get; private set; }
        public FuelType? FuelType { get; private set; }
        public int FuelTypeId { get; private set; }

        internal Nozzle(int tankId , int pumpId, int number, decimal amount, decimal volume, decimal totalizer, decimal price , string? readerNumber = null)
        {
            Validate(tankId, pumpId, number, amount, volume);
            
            TankId = tankId;
            PumpId = pumpId;
            Number = number;
            Status = NozzleStatus.Disabled;
            Amount = amount;
            Volume = volume;
            Totalizer = totalizer;
            ReaderNumber = readerNumber;
            Price = price;
        }

        private void Validate(int tankId, int pumpId, int number, decimal amount, decimal volume)
        {
            if (tankId <= 0) throw new ArgumentException("Tank id must be greater than 0");
            if (pumpId <= 0) throw new ArgumentException("Pump id must be greater than 0");
            if (number <= 0) throw new ArgumentException("Number must be greater than 0");
            if (amount < 0) throw new ArgumentException("Amount must be greater than or equal to 0");
            if (volume < 0) throw new ArgumentException("Volume must be greater than or equal to 0");
        }
    
        public void ChangePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Price must be greater than 0");

            if (Price == newPrice)
                return;

            Price = newPrice;
            UpdatedAt = DateTimeCulture.Now;    
        }

        public void Update(decimal amount, decimal volume, decimal totalizer, string? readerNumber)
        {
            Validate(TankId, PumpId, Number, amount, volume);

            Amount = amount;
            Volume = volume;
            Totalizer = totalizer;
            ReaderNumber = readerNumber;
            UpdatedAt = DateTimeCulture.Now;

            // TODO : After this operation we have to add this to activity log.
        }
    }
}
