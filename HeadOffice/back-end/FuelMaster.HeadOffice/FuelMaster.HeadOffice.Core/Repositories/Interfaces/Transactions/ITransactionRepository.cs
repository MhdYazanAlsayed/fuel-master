using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Transactions;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface ITransactionRepository : IRepository<Transaction>, IScopedDependency
{
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
    Task<Transaction?> DetailsAsync(long id, bool includeNozzle = false, bool includeEmployee = false, bool includeStation = false, bool includePump = false);
    ITransactionReadQuery UseScopeFilter();
}

