//using FuelMaster.HeadOffice.Core.Entities;
//using FuelMaster.HeadOffice.Core.Enums;

//namespace FuelMaster.HeadOffice.Core.Models.Responses.Reports
//{
//    public class DashboardReportsDto
//    {
//        public List<TankLevelReportDto> TankLevels { get; set; } = new();
//        public List<DailySalesReportDto> DailySales { get; set; } = new();
//        public List<StationsComparisonReportDto> StationsComparison { get; set; } = new();
//        public List<SalesByPaymentMethodReportDto> SalesByPaymentMethod { get; set; } = new();
//        public List<EmployeePerformanceReportDto> EmployeePerformance { get; set; } = new();
//        public List<MonthlySalesTrendReportDto> MonthlySalesTrend { get; set; } = new();
//    }

//    public class TankLevelReportDto
//    {
//        public int TankId { get; set; }
//        public int StationId { get; set; }
//        public Entities.Station? Station { get; set; }
//        public int Number { get; set; }
//        public FuelType FuelType { get; set; }
//        public decimal Capacity { get; set; }
//        public decimal CurrentVolume { get; set; }
//        public decimal CurrentLevel { get; set; }
//        public decimal UtilizationPercentage { get; set; }
//    }

//    public class DailySalesReportDto
//    {
//        public int Hour { get; set; }
//        public decimal TotalVolume { get; set; }
//        public decimal TotalAmount { get; set; }
//    }

//    public class StationsComparisonReportDto
//    {
//        public int? StationId { get; set; }
//        public string StationName { get; set; } = string.Empty;
//        public decimal TotalVolume { get; set; }
//        public decimal TotalAmount { get; set; }
//    }

//    public class SalesByPaymentMethodReportDto
//    {
//        public PaymentMethod PaymentMethod { get; set; }
//        public decimal TotalVolume { get; set; }
//        public decimal TotalAmount { get; set; }
//    }

//    public class EmployeePerformanceReportDto
//    {
//        public int EmployeeId { get; set; }
//        public string FullName { get; set; } = string.Empty;
//        public decimal TotalVolume { get; set; }
//        public decimal TotalAmount { get; set; }
//    }

//    public class MonthlySalesTrendReportDto
//    {
//        public int Year { get; set; }
//        public int Month { get; set; }
//        public decimal TotalVolume { get; set; }
//        public decimal TotalAmount { get; set; }
//    }
//}
