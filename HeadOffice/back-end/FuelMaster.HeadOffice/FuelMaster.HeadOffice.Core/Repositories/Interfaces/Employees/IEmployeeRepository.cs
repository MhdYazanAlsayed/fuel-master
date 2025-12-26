using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Employees;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>, IScopedDependency
{
    // This in order to apply filter strategies for employee's scopes
    IEmployeeReadQuery AsReadQuery();
    Task<Employee?> DetailsAsync(int id, bool includeRole = false, bool includeStation = false, bool includeArea = false, bool includeCity = false, bool includeUser = false);
    Task<List<Employee>> GetAllAsync(bool includeRole = false, bool includeStation = false, bool includeArea = false, bool includeCity = false, bool includeUser = false);
    Task<(List<Employee>, int)> GetPaginationAsync(int page, int pageSize, bool includeRole = false, bool includeStation = false, bool includeArea = false, bool includeCity = false, bool includeUser = false);
    Task<Employee?> GetByCardNumberAsync(string cardNumber, bool includeRole = false, bool includeStation = false, bool includeArea = false, bool includeCity = false, bool includeUser = false);
}

