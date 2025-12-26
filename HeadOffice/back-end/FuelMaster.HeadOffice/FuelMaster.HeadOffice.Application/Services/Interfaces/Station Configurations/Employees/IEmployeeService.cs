using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.Results;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business;

public interface IEmployeeService : IScopedDependency
{
    Task<IEnumerable<EmployeeResult>> GetAllAsync();
    Task<PaginationDto<EmployeeResult>> GetPaginationAsync(int currentPage);
    Task<ResultDto<EmployeeResult>> CreateAsync(CreateEmployeeDto dto);
    Task<ResultDto<EmployeeResult>> UpdateAsync(int id, UpdateEmployeeDto dto);
    Task<EmployeeResult?> DetailsAsync(int id);
    Task<EmployeeResult?> GetByCardNumberAsync(string cardNumber);
}