using FuelMaster.HeadOffice.Core.Entities;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces.Transactions;

public interface ITransactionReadQuery 
{
    ITransactionReadQuery ApplyFilter(Scope scope, int? cityId, int? areaId, int? stationId);
    Task<(List<Transaction> Data, int TotalCount)> GetPaginationAsync(int page, int pageSize, bool includeNozzle = false, bool includeEmployee = false, bool includeStation = false, bool includePump = false);
}