using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.ResultQuery;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces.Employees;

public interface IEmployeeReadQuery 
{
    IEmployeeReadQuery ApplyFilter(Scope scope, int? cityId, int? areaId, int? stationId);
    Task<List<Employee>> GetAllAsync(bool includeRole = false, bool includeStation = false, bool includeArea = false, bool includeCity = false, bool includeUser = false);
    Task<(List<Employee>, int)> GetPaginationAsync(int page, int pageSize, bool includeRole = false, bool includeStation = false, bool includeArea = false, bool includeCity = false, bool includeUser = false, bool ignoreQueryFilter = false);
}