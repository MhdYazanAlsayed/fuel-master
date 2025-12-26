using FuelMaster.HeadOffice.Core.Entities;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces.Transactions;

public interface ITransactionReadQuery 
{
    ITransactionReadQuery ApplyFilter(Scope scope, int? cityId, int? areaId, int? stationId);
    Task<(List<Transaction> Data, int TotalCount)> GetPaginationAsync(
        int page, 
        int pageSize, 
        // Filters,
        int? areaId = null,
        int? cityId = null,
        int? stationId = null,
        int? nozzleId = null,
        int? pumpId = null,
        int? employeeId = null,
        DateTime? from = null,
        DateTime? to = null,
        
        // Includes
        bool includeNozzle = false, 
        bool includeEmployee = false, 
        bool includeStation = false, 
        bool includePump = false);

    Task<List<Transaction>> GetAllAsync (
        DateTime from,
        DateTime to,
        // Filters,
        int? areaId = null,
        int? cityId = null,
        int? stationId = null,
        int? nozzleId = null,
        int? pumpId = null,
        int? employeeId = null,
        
        // Includes
        bool includeNozzle = false, 
        bool includeEmployee = false, 
        bool includeStation = false, 
        bool includePump = false);
}