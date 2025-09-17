using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Core.Models.Responses.Reports
{
    public class GetTransactionReportResponse
    {
        public string? StationName { get; set; }
        public decimal Vat { get; set; }
        public Pump? Pump { get; set; }
        public Nozzle? Nozzle { get; set; }
        public Employee? Employee { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Volume { get; set; }
        public decimal Totalizer { get; set; }
        public decimal TotalizerAfter { get; set; }
        public DateTime DateTime { get; set; }
    }
}
