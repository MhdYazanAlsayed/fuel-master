using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Reports.Dashboard;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Reports.Dashboard.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Transactions;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Reports.Dashboard;

public class DashboardReportService : IDashboardReportService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITankRepository _tankRepository;
    private readonly IStationRepository _stationRepository;
    private readonly ISigninService _signinService;
    private readonly ILogger<DashboardReportService> _logger;

    public DashboardReportService(
        ITransactionRepository transactionRepository,
        ITankRepository tankRepository,
        IStationRepository stationRepository,
        ISigninService signinService,
        ILogger<DashboardReportService> logger)
    {
        _transactionRepository = transactionRepository;
        _tankRepository = tankRepository;
        _stationRepository = stationRepository;
        _signinService = signinService;
        _logger = logger;
    }

    public async Task<ResultDto<DashboardReportsResult>> GetDashboardReportsAsync()
    {
        try
        {
            var stationIds = await GetStationIdsByScopeAsync();
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);
            var twelveMonthsAgo = DateTime.Now.AddMonths(-12);

            var dashboardReports = new DashboardReportsResult();

            // 1. Tank Levels Report
            dashboardReports.TankLevels = await GetTankLevelsReportAsync(stationIds);

            // 2. Daily Sales Report (grouped by hour for current day)
            dashboardReports.DailySales = await GetDailySalesReportAsync(stationIds, today, tomorrow);

            // 3. Stations Comparison Report
            dashboardReports.StationsComparison = await GetStationsComparisonReportAsync(stationIds);

            // 4. Sales by Payment Method Report
            dashboardReports.SalesByPaymentMethod = await GetSalesByPaymentMethodReportAsync(stationIds);

            // 5. Employee Performance Report
            dashboardReports.EmployeePerformance = await GetEmployeePerformanceReportAsync(stationIds);

            // 6. Monthly Sales Trend Report (last 12 months)
            dashboardReports.MonthlySalesTrend = await GetMonthlySalesTrendReportAsync(stationIds, twelveMonthsAgo);

            return Result.Success(dashboardReports);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard reports");
            return Result.Failure<DashboardReportsResult>(ex.Message);
        }
    }

    private async Task<List<int>?> GetStationIdsByScopeAsync()
    {
        var scope = _signinService.GetCurrentScope();
        var cityId = _signinService.GetCurrentCityId();
        var areaId = _signinService.GetCurrentAreaId();
        var stationId = _signinService.GetCurrentStationId();

        if (scope == Scope.ALL)
        {
            // Return null to indicate all stations
            return null;
        }

        if (scope == Scope.City && cityId.HasValue)
        {
            var stations = await _stationRepository.GetAllAsync();
            return stations.Where(s => s.CityId == cityId.Value).Select(s => s.Id).ToList();
        }

        if (scope == Scope.Area && areaId.HasValue)
        {
            var stations = await _stationRepository.GetAllAsync();
            return stations.Where(s => s.AreaId == areaId.Value).Select(s => s.Id).ToList();
        }

        if ((scope == Scope.Station || scope == Scope.Self) && stationId.HasValue)
        {
            return new List<int> { stationId.Value };
        }

        // If scope is not recognized or required IDs are missing, return empty list
        return new List<int>();
    }

    private async Task<List<TankLevelReportResult>> GetTankLevelsReportAsync(List<int>? stationIds)
    {
        try
        {
            var tanks = await _tankRepository.GetAllAsync(includeStation: true, includeFuelType: true);

            var filteredTanks = stationIds != null
                ? tanks.Where(t => stationIds.Contains(t.StationId)).ToList()
                : tanks.ToList();

            return filteredTanks.Select(tank => new TankLevelReportResult
            {
                TankId = tank.Id,
                StationId = tank.StationId,
                StationName = tank.Station != null
                    ? (LocalizationUtilities.IsArabic() ? tank.Station.ArabicName : tank.Station.EnglishName)
                    : null,
                Number = tank.Number,
                FuelTypeName = tank.FuelType != null
                    ? (LocalizationUtilities.IsArabic() ? tank.FuelType.ArabicName : tank.FuelType.EnglishName)
                    : "Unknown",
                Capacity = tank.Capacity,
                CurrentVolume = tank.CurrentVolume,
                CurrentLevel = tank.CurrentLevel,
                UtilizationPercentage = tank.Capacity > 0 ? (tank.CurrentVolume / tank.Capacity) * 100 : 0
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tank levels report");
            return new List<TankLevelReportResult>();
        }
    }

    private async Task<List<DailySalesReportResult>> GetDailySalesReportAsync(List<int>? stationIds, DateTime today, DateTime tomorrow)
    {
        try
        {
            var transactions = await _transactionRepository.GetAllAsync(
                from: today,
                to: tomorrow,
                stationId: null, // Get all, then filter by stationIds
                includeStation: true);

            // Filter by station IDs if provided
            if (stationIds != null && stationIds.Any())
            {
                transactions = transactions.Where(t => stationIds.Contains(t.StationId)).ToList();
            }

            var groupedByHour = transactions
                .GroupBy(t => t.DateTime.Hour)
                .Select(g => new DailySalesReportResult
                {
                    Hour = g.Key,
                    TotalVolume = g.Sum(t => t.Volume),
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .OrderBy(x => x.Hour)
                .ToList();

            // Fill in missing hours with zero values
            var allHours = Enumerable.Range(0, 24).ToList();
            var existingHours = groupedByHour.Select(x => x.Hour).ToList();
            var missingHours = allHours.Except(existingHours);

            foreach (var hour in missingHours)
            {
                groupedByHour.Add(new DailySalesReportResult
                {
                    Hour = hour,
                    TotalVolume = 0,
                    TotalAmount = 0
                });
            }

            return groupedByHour.OrderBy(x => x.Hour).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting daily sales report");
            return new List<DailySalesReportResult>();
        }
    }

    private async Task<List<StationsComparisonReportResult>> GetStationsComparisonReportAsync(List<int>? stationIds)
    {
        try
        {
            var transactions = await _transactionRepository.GetAllAsync(
                from: DateTime.MinValue,
                to: DateTime.MaxValue,
                stationId: null, // Get all, then filter by stationIds
                includeStation: true);

            // Filter by station IDs if provided
            if (stationIds != null && stationIds.Any())
            {
                transactions = transactions.Where(t => stationIds.Contains(t.StationId)).ToList();
            }

            var grouped = transactions
                .Where(t => t.Station != null)
                .GroupBy(t => new
                {
                    t.StationId,
                    StationName = LocalizationUtilities.IsArabic() ? t.Station!.ArabicName : t.Station!.EnglishName
                })
                .Select(g => new StationsComparisonReportResult
                {
                    StationId = g.Key.StationId,
                    StationName = g.Key.StationName,
                    TotalVolume = g.Sum(t => t.Volume),
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToList();

            return grouped;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting stations comparison report");
            return new List<StationsComparisonReportResult>();
        }
    }

    private async Task<List<SalesByPaymentMethodReportResult>> GetSalesByPaymentMethodReportAsync(List<int>? stationIds)
    {
        try
        {
            var transactions = await _transactionRepository.GetAllAsync(
                from: DateTime.MinValue,
                to: DateTime.MaxValue,
                stationId: null); // Get all, then filter by stationIds

            // Filter by station IDs if provided
            if (stationIds != null && stationIds.Any())
            {
                transactions = transactions.Where(t => stationIds.Contains(t.StationId)).ToList();
            }

            var grouped = transactions
                .GroupBy(t => t.PaymentMethod)
                .Select(g => new SalesByPaymentMethodReportResult
                {
                    PaymentMethod = g.Key.ToString(),
                    TotalVolume = g.Sum(t => t.Volume),
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToList();

            return grouped;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sales by payment method report");
            return new List<SalesByPaymentMethodReportResult>();
        }
    }

    private async Task<List<EmployeePerformanceReportResult>> GetEmployeePerformanceReportAsync(List<int>? stationIds)
    {
        try
        {
            var transactions = await _transactionRepository.GetAllAsync(
                from: DateTime.MinValue,
                to: DateTime.MaxValue,
                stationId: null, // Get all, then filter by stationIds
                includeEmployee: true);

            // Filter by station IDs if provided
            if (stationIds != null && stationIds.Any())
            {
                transactions = transactions.Where(t => stationIds.Contains(t.StationId)).ToList();
            }

            var grouped = transactions
                .Where(t => t.Employee != null)
                .GroupBy(t => new { t.EmployeeId, t.Employee!.FullName })
                .Select(g => new EmployeePerformanceReportResult
                {
                    EmployeeId = g.Key.EmployeeId,
                    FullName = g.Key.FullName,
                    TotalVolume = g.Sum(t => t.Volume),
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToList();

            return grouped;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employee performance report");
            return new List<EmployeePerformanceReportResult>();
        }
    }

    private async Task<List<MonthlySalesTrendReportResult>> GetMonthlySalesTrendReportAsync(List<int>? stationIds, DateTime fromDate)
    {
        try
        {
            var transactions = await _transactionRepository.GetAllAsync(
                from: fromDate,
                to: DateTime.MaxValue,
                stationId: null); // Get all, then filter by stationIds

            // Filter by station IDs if provided
            if (stationIds != null && stationIds.Any())
            {
                transactions = transactions.Where(t => stationIds.Contains(t.StationId)).ToList();
            }

            var grouped = transactions
                .GroupBy(t => new { t.DateTime.Year, t.DateTime.Month })
                .Select(g => new MonthlySalesTrendReportResult
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalVolume = g.Sum(t => t.Volume),
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            return grouped;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting monthly sales trend report");
            return new List<MonthlySalesTrendReportResult>();
        }
    }
}

