namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Reports.Dashboard.Results;

public class DashboardReportsResult
{
    public List<TankLevelReportResult> TankLevels { get; set; } = new();
    public List<DailySalesReportResult> DailySales { get; set; } = new();
    public List<StationsComparisonReportResult> StationsComparison { get; set; } = new();
    public List<SalesByPaymentMethodReportResult> SalesByPaymentMethod { get; set; } = new();
    public List<EmployeePerformanceReportResult> EmployeePerformance { get; set; } = new();
    public List<MonthlySalesTrendReportResult> MonthlySalesTrend { get; set; } = new();
}

public class TankLevelReportResult
{
    public int TankId { get; set; }
    public int StationId { get; set; }
    public string? StationName { get; set; }
    public int Number { get; set; }
    public string? FuelTypeName { get; set; }
    public decimal Capacity { get; set; }
    public decimal CurrentVolume { get; set; }
    public decimal CurrentLevel { get; set; }
    public decimal UtilizationPercentage { get; set; }
}

public class DailySalesReportResult
{
    public int Hour { get; set; }
    public decimal TotalVolume { get; set; }
    public decimal TotalAmount { get; set; }
}

public class StationsComparisonReportResult
{
    public int? StationId { get; set; }
    public string StationName { get; set; } = string.Empty;
    public decimal TotalVolume { get; set; }
    public decimal TotalAmount { get; set; }
}

public class SalesByPaymentMethodReportResult
{
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal TotalVolume { get; set; }
    public decimal TotalAmount { get; set; }
}

public class EmployeePerformanceReportResult
{
    public int EmployeeId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public decimal TotalVolume { get; set; }
    public decimal TotalAmount { get; set; }
}

public class MonthlySalesTrendReportResult
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalVolume { get; set; }
    public decimal TotalAmount { get; set; }
}

