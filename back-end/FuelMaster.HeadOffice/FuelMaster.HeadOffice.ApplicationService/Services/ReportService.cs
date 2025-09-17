using AutoMapper;
using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Contracts.Services;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Deliveries;
using FuelMaster.HeadOffice.Core.Models.Requests.Reports;
using FuelMaster.HeadOffice.Core.Models.Requests.TankTransactions;
using FuelMaster.HeadOffice.Core.Models.Requests.Transactions;
using FuelMaster.HeadOffice.Core.Models.Responses.Reports;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.ApplicationService.Services
{
    public class ReportService : IReportService
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IDeliveryService _deliveryService;
        private readonly ITankTransactionService _tankTransaction;
        private readonly FuelMasterDbContext _context;
        private readonly IAuthorization _authorization;
        
        public ReportService(
        IContextFactory<FuelMasterDbContext> contextFactory,
        IMapper mapper,
        ITransactionService transactionService,
        IDeliveryService deliveryService,
        ITankTransactionService tankTransaction, 
        IAuthorization authorization)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            _deliveryService = deliveryService;
            _tankTransaction = tankTransaction;
            _context = contextFactory.CurrentContext;
            _authorization = authorization;
        }

        public async Task<DashboardReportResponse> GetDashboardReportAsync(int? stationId)
        {
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);
            
            // Get tanks for current station
            var tanks = await _context.Tanks
                .Include(x => x.Station)
                .Where(x => !stationId.HasValue || x.StationId == stationId)
                .ToListAsync();

            // Create request for reports
            var reportRequest = new GetStoredProcedureReportRequest
            {
                StationId = stationId,
                StartDate = today,
                EndDate = tomorrow
            };

            // Get all reports
            var stationsReport = await GetStationsReportAsync(reportRequest);
            var nozzlesReport = await GetNozzlesReportAsync(reportRequest);
            var employeesReport = await GetEmployeesReportAsync(reportRequest);
            var paymentTransactionsReport = await GetPaymentTransactionsReportAsync(reportRequest);

            return new DashboardReportResponse
            {
                Tanks = tanks,
                StationsReport = stationsReport,
                NozzlesReport = nozzlesReport,
                EmployeesReport = employeesReport,
                PaymentTransactionsReport = paymentTransactionsReport
            };
        }

        public async Task<DashboardReportsDto> GetDashboardReportsAsync()
        {
            int? stationId = await _authorization.TryToGetStationIdAsync();

            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);
            var twelveMonthsAgo = DateTime.Now.AddMonths(-12);

            var dashboardReports = new DashboardReportsDto();

            // 1. Tank Levels Report
            dashboardReports.TankLevels = await _context.Tanks
                .AsNoTracking()
                .Include(x => x.Station)
                .Where(x => !stationId.HasValue || x.StationId == stationId)
                .Select(tank => new TankLevelReportDto
                {
                    TankId = tank.Id,
                    Number = tank.Number,
                    StationId = tank.StationId,
                    Station = tank.Station,
                    FuelType = tank.FuelType,
                    Capacity = tank.Capacity,
                    CurrentVolume = tank.CurrentVolume,
                    CurrentLevel = tank.CurrentLevel,
                    UtilizationPercentage = tank.Capacity > 0 ? (tank.CurrentVolume / tank.Capacity) * 100 : 0
                })
                .ToListAsync();

            // 2. Daily Sales Report (grouped by hour for current day)
            dashboardReports.DailySales = await _context.Transactions
                .AsNoTracking()
                .Where(t => t.DateTime >= today && t.DateTime < tomorrow && (!stationId.HasValue || t.StationId == stationId))
                .GroupBy(t => t.DateTime.Hour)
                .Select(g => new DailySalesReportDto
                {
                    Hour = g.Key,
                    TotalVolume = g.Sum(t => t.Volume),
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .OrderBy(x => x.Hour)
                .ToListAsync();

            // 3. Stations Comparison Report
            dashboardReports.StationsComparison = await _context.Transactions
                .AsNoTracking()
                .Include(t => t.Station)
                .Where(x => !stationId.HasValue || x.StationId == stationId)
                .GroupBy(t => new { t.StationId, StationName = t.Station != null ? 
                    (LocalizationUtilities.IsArabic() ? t.Station.ArabicName : t.Station.EnglishName) : "Unknown Station" })
                .Select(g => new StationsComparisonReportDto
                {
                    StationId = g.Key.StationId,
                    StationName = g.Key.StationName,
                    TotalVolume = g.Sum(t => t.Volume),
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToListAsync();

            // 4. Sales by Payment Method Report
            dashboardReports.SalesByPaymentMethod = await _context.Transactions
                .AsNoTracking()
                .Where(x => !stationId.HasValue || x.StationId == stationId)
                .GroupBy(t => t.PaymentMethod)
                .Select(g => new SalesByPaymentMethodReportDto
                {
                    PaymentMethod = g.Key,
                    TotalVolume = g.Sum(t => t.Volume),
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToListAsync();

            // 5. Employee Performance Report
            dashboardReports.EmployeePerformance = await _context.Transactions
                .AsNoTracking()
                .Include(t => t.Employee)
                .Where(t => t.Employee != null)
                .Where(x => !stationId.HasValue || x.StationId == stationId)
                .GroupBy(t => new { t.EmployeeId, t.Employee!.FullName })
                .Select(g => new EmployeePerformanceReportDto
                {
                    EmployeeId = g.Key.EmployeeId,
                    FullName = g.Key.FullName,
                    TotalVolume = g.Sum(t => t.Volume),
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToListAsync();

            // 6. Monthly Sales Trend Report (last 12 months)
            dashboardReports.MonthlySalesTrend = await _context.Transactions
                .AsNoTracking()
                .Where(t => t.DateTime >= twelveMonthsAgo && (!stationId.HasValue || t.StationId == stationId))
                .GroupBy(t => new { t.DateTime.Year, t.DateTime.Month })
                .Select(g => new MonthlySalesTrendReportDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalVolume = g.Sum(t => t.Volume),
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            return dashboardReports;
        }

        public async Task<PaginationDto<TankTransaction>> GetTankTransactionReportAsync(GetTankTransactionPaginationDto dto)
        {
            return await _tankTransaction.GetPaginationAsync(dto);
        }

        public async Task<PaginationDto<Delivery>> GetDeliveryReportAsync(GetDeliveriesPaginationDto dto)
        {
            return await _deliveryService.GetPaginationAsync(dto);
        }

        public async Task<GetRealTimeReportResponse> GetRealTimeReportAsync(int? stationId)
        {
            var nozzles = await GetRealTimeNozzlesAsync(stationId);
            var tanks = await GetRealTimeTanksAsync(stationId);

            return new(nozzles , tanks);
        }

        public async Task<PaginationDto<GetTransactionReportResponse>> GetTransactionReportAsync(GetTransactionPaginationDto dto)
        {
            dto.StationId = (await _authorization.TryToGetStationIdAsync()) ?? dto.StationId;
            
            var transaction = await _transactionService.GetPaginationAsync(dto , true);

            var newTransactions = transaction.Data.Select(x =>
            {
                return new GetTransactionReportResponse()
                {
                    Amount = x.Amount,
                    DateTime = x.CreatedAt , //
                    Nozzle = x.Nozzle, //
                    Pump = x.Nozzle!.Pump , //
                    PaymentMethod = x.PaymentMethod,
                    Price = x.Price,
                    StationName = !LocalizationUtilities.IsArabic() ? //
                        x.Station?.EnglishName : 
                        x.Station?.ArabicName ,
                    Totalizer = x.Totalizer,
                    TotalizerAfter = x.TotalizerAfter,
                    Vat = x.Amount * 0.15m,
                    Volume = x.Volume,
                    Employee = x.Employee,
                };
            })
            .ToList();

            return new 
                PaginationDto<GetTransactionReportResponse>(newTransactions, transaction.Pages);
        }

        public async Task<StoredProcedureReportsResponse> GetStoredProcedureReportsAsync(GetStoredProcedureReportRequest request)
        {
            request.StationId = (await _authorization.TryToGetStationIdAsync()) ?? request.StationId;

            if (request.StartDate is null) 
            {
                request.StartDate = DateTime.Now.AddDays(-10);
            }

            if (request.EndDate is null)
            {
                request.EndDate = DateTime.Now;
            }

            var response = new StoredProcedureReportsResponse();

            try
            {
                // Execute all reports using Entity Framework queries
                response.EmployeesReport = await GetEmployeesReportAsync(request);
                response.PaymentTransactionsReport = await GetPaymentTransactionsReportAsync(request);
                response.StationsReport = await GetStationsReportAsync(request);
                response.NozzlesReport = await GetNozzlesReportAsync(request);
            }
            catch (Exception ex)
            {
                // Log the error and return empty results
                Console.WriteLine($"Error executing reports: {ex.Message}");
            }

            return response;
        }

        private async Task<List<EmployeesReportResponse>> GetEmployeesReportAsync(GetStoredProcedureReportRequest request)
        {
            var query = _context.Transactions
                .Include(x => x.Employee)
                .Include(x => x.Station)
                .AsQueryable();

            // Apply filters
            if (request.StationId.HasValue)
                query = query.Where(x => x.StationId == request.StationId);

            if (request.StartDate.HasValue)
                query = query.Where(x => x.CreatedAt >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                query = query.Where(x => x.CreatedAt <= request.EndDate.Value);

            var results = await query
                .GroupBy(x => new { x.Employee!.FullName, x.Station!.ArabicName, x.Station.EnglishName })
                .Select(g => new EmployeesReportResponse
                {
                    FullName = g.Key.FullName,
                    StationArabicName = g.Key.ArabicName,
                    StationEnglishName = g.Key.EnglishName,
                    Volume = g.Sum(x => x.Volume),
                    Amount = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            return results;
        }

        private async Task<List<PaymentTransactionsReportResponse>> GetPaymentTransactionsReportAsync(GetStoredProcedureReportRequest request)
        {
            var query = _context.Transactions
                .Include(x => x.Station)
                .AsQueryable();

            // Apply filters
            if (request.StationId.HasValue)
                query = query.Where(x => x.StationId == request.StationId);

            if (request.StartDate.HasValue)
                query = query.Where(x => x.CreatedAt >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                query = query.Where(x => x.CreatedAt <= request.EndDate.Value);

            var results = await query
                .GroupBy(x => new { x.PaymentMethod, x.Station!.ArabicName, x.Station.EnglishName })
                .Select(g => new PaymentTransactionsReportResponse
                {
                    ArabicName = g.Key.ArabicName,
                    EnglishName = g.Key.EnglishName,
                    PaymentMethod = g.Key.PaymentMethod,
                    TransactionCount = g.Count(),
                    Volume = g.Sum(x => x.Volume),
                    Amount = g.Sum(x => x.Amount)
                })
                .OrderBy(x => x.ArabicName)
                .ThenBy(x => x.EnglishName)
                .ToListAsync();

            return results;
        }

        private async Task<List<StationsReportResponse>> GetStationsReportAsync(GetStoredProcedureReportRequest request)
        {
            var query = _context.Transactions
                .Include(x => x.Station)
                .AsQueryable();

            // Apply filters
            if (request.StationId.HasValue)
                query = query.Where(x => x.StationId == request.StationId);

            if (request.StartDate.HasValue)
                query = query.Where(x => x.CreatedAt >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                query = query.Where(x => x.CreatedAt <= request.EndDate.Value);

            var results = await query
                .GroupBy(x => new { x.Station!.ArabicName, x.Station.EnglishName })
                .Select(g => new StationsReportResponse
                {
                    ArabicName = g.Key.ArabicName,
                    EnglishName = g.Key.EnglishName,
                    Volume = g.Sum(x => x.Volume),
                    Amount = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            return results;
        }

        private async Task<List<NozzlesReportResponse>> GetNozzlesReportAsync(GetStoredProcedureReportRequest request)
        {
            var query = _context.Transactions
                .Include(x => x.Nozzle)
                .ThenInclude(x => x!.Tank)
                .Include(x => x.Station)
                .AsQueryable();

            // Apply filters
            if (request.StationId.HasValue)
                query = query.Where(x => x.StationId == request.StationId);

            if (request.StartDate.HasValue)
                query = query.Where(x => x.CreatedAt >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                query = query.Where(x => x.CreatedAt <= request.EndDate.Value);

            var results = await query
                .GroupBy(x => new { 
                    x.Station!.ArabicName, 
                    x.Station.EnglishName, 
                    x.Nozzle!.Number, 
                    x.Nozzle.Tank!.FuelType 
                })
                .Select(g => new NozzlesReportResponse
                {
                    StationArabicName = g.Key.ArabicName,
                    StationEnglishName = g.Key.EnglishName,
                    Number = g.Key.Number,
                    FuelType = g.Key.FuelType.ToString(),
                    Volume = g.Sum(x => x.Volume),
                    Amount = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            return results;
        }

        // Local methods 
        private async Task<List<GetRealTimeReportResposne_Nozzle>> GetRealTimeNozzlesAsync (int? stationId)
        {
            stationId = (await _authorization.TryToGetStationIdAsync()) ?? stationId;

            var nozzles = await _context.Nozzles
                .Include(x => x.Pump)
                .Include(x => x.Tank)
                .Include(x => x.Tank!.Station)
                .Where(x => !stationId.HasValue || x.Tank!.StationId == stationId)
                .ToListAsync();

            var nozzleList = nozzles.Select(nozzle => new GetRealTimeReportResposne_Nozzle()
            {
                Price = nozzle.Price,
                Amount = nozzle.Amount,
                FuelType = nozzle.Tank!.FuelType,
                Number = nozzle.Number,
                Pump = nozzle.Pump,
                Status = nozzle.Status,
                Volume = nozzle.Volume
            })
            .ToList();

            return nozzleList;
        }

        private async Task<List<GetRealTimeReportResposne_Tanks>> GetRealTimeTanksAsync (int? stationId)
        {
            stationId = (await _authorization.TryToGetStationIdAsync()) ?? stationId;

            var tanks = await _context.Tanks
                .Where(x => !stationId.HasValue || x.StationId == stationId)
                .ToListAsync();

            return _mapper.Map<List<GetRealTimeReportResposne_Tanks>>(tanks);
        }
    }
}
