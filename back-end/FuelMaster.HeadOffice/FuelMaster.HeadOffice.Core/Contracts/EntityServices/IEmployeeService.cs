//using FuelMaster.HeadOffice.Core.Contracts.Markers;
//using FuelMaster.HeadOffice.Core.Entities;
//using FuelMaster.HeadOffice.Core.Models.Dtos;
//using FuelMaster.HeadOffice.Core.Models.Requests.Accounts;
//using FuelMaster.HeadOffice.Core.Models.Requests.Employees;

//namespace FuelMaster.HeadOffice.Core.Contracts.Entities
//{
//    public interface IEmployeeService : IScopedDependency
//    {
//        Task<ResultDto> EditPasswordAsync(int employeeId, EditPasswordDto dto);
//        Task<ResultDto<Employee>> CreateAsync(CreateEmployeeDto dto);
//        Task<ResultDto<Employee>> EditAsync(int employeeId , EditEmployeeDto dto);
//        Task<PaginationDto<Employee>> GetPaginationAsync(GetPaginationEmployeeDto dto);
//        Task<Employee?> DetailsAsync (int employeeId);
//        Task<List<Employee>> GetAsync(int? stationId);
//    }
//}
