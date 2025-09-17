using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Core.Entities
{
    public class Transaction : EntityBase<long>
    {
        public string UId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        
        // Unit price of fuel (e.g. price per liter = $1.2).
        public decimal Price { get; set; }
        public int NozzleId { get; set; }
        public Nozzle? Nozzle { get; set; }

        // The financial amount of the transaction (total price paid).
        public decimal Amount { get; set; }

        // Quantity of fuel sold (in liters or cubic meters).
        public decimal Volume { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public decimal Totalizer { get; set; }
        public decimal TotalizerAfter { get; set; }
        public int StationId { get; set; }
        public Station? Station { get; set; }
        public DateTime DateTime { get; set; }

        public Transaction(string uId , PaymentMethod paymentMethod, decimal price, int nozzleId, decimal amount, decimal volume, int employeeId, decimal totalizer, decimal totalizerAfter , int stationId , DateTime dateTime)
        {
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
