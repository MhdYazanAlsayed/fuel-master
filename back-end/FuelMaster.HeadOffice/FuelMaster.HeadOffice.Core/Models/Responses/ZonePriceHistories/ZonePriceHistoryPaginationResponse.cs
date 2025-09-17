using FuelMaster.HeadOffice.Core.Enums;

namespace FuelMaster.HeadOffice.Core.Models.Responses.ZonePriceHistories
{
    public class ZonePriceHistoryPaginationResponse
    {
        public FuelType FuelType { get; set; }
        public string UserName { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public decimal PriceBeforeChange { get; set; }

    }
}
