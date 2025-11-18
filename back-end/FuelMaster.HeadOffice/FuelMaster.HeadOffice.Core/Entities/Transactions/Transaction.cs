using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Transaction : Entity<long>
    {
        public string UId { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        
        // Unit price of fuel (e.g. price per liter = $1.2).
        public decimal Price { get; private set; }
        public int NozzleId { get; private set; }
        public Nozzle? Nozzle { get; private set; }

        // The financial amount of the transaction (total price paid).
        public decimal Amount { get; private set; }

        // Quantity of fuel sold (in liters or cubic meters).
        public decimal Volume { get; private set; }
        public int EmployeeId { get; private set; }
        public Employee? Employee { get; private set; }
        public decimal Totalizer { get; private set; }
        public decimal TotalizerAfter { get; private set; }
        public int StationId { get; private set; }
        public Station? Station { get; private set; }
        public DateTime DateTime { get; private set; }

        public Transaction(string uId , PaymentMethod paymentMethod, decimal price, int nozzleId, decimal amount, decimal volume, int employeeId, decimal totalizer, decimal totalizerAfter , int stationId , DateTime dateTime)
        {
            if (price <= 0) throw new ArgumentException("Price must be greater than 0");
            if (amount <= 0) throw new ArgumentException("Amount must be greater than 0");
            if (volume <= 0) throw new ArgumentException("Volume must be greater than 0");
            if (totalizer <= 0) throw new ArgumentException("Totalizer must be greater than 0");
            if (totalizerAfter <= 0) throw new ArgumentException("TotalizerAfter must be greater than 0");
            if (stationId <= 0) throw new ArgumentException("StationId must be greater than 0");
            if (dateTime <= DateTime.MinValue) throw new ArgumentException("DateTime must be greater than 0");

            UId = uId;
            PaymentMethod = paymentMethod;
            Price = price;
            NozzleId = nozzleId;
            Amount = amount;
            Volume = volume;
            EmployeeId = employeeId;
            Totalizer = totalizer;
            TotalizerAfter = totalizerAfter;
            StationId = stationId;
            DateTime = dateTime;
        }
    }
}
