//using FuelMaster.HeadOffice.Core.Contracts.Authentication;
//using FuelMaster.HeadOffice.Core.Contracts.Database;
//using FuelMaster.HeadOffice.Core.Contracts.Entities;
//using FuelMaster.HeadOffice.Core.Contracts.Services;
//using FuelMaster.HeadOffice.Core.Entities;
//using FuelMaster.HeadOffice.Core.Helpers;
//using FuelMaster.HeadOffice.Core.Models.Dtos;
//using FuelMaster.HeadOffice.Core.Models.Requests.Accounts;
//using FuelMaster.HeadOffice.Core.Models.Requests.Employees;
//using FuelMaster.HeadOffice.Core.Resources;
//using FuelMaster.HeadOffice.Infrastructure.Contexts;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace FuelMaster.HeadOffice.ApplicationService.Entities
//{
//    public class EmployeeService : IEmployeeService
//    {
//        private readonly FuelMasterDbContext _context;
//        private readonly IUserService _userService;
//        private readonly IAccountService _accountService;       
//        private readonly ISigninService _authorization;
//        private readonly ILogger<EmployeeService> _logger;
//        private readonly ICacheService _cacheService;

//        public EmployeeService(
//            IContextFactory<FuelMasterDbContext> contextFactory, 
//            IUserService userService, 
//            IAccountService accountService, 
//            ISigninService authorization,
//            ILogger<EmployeeService> logger,
//            ICacheService cacheService)
//        {
//            _context = contextFactory.CurrentContext;
//            _userService = userService;
//            _accountService = accountService;
//            _authorization = authorization;
//            _logger = logger;
//            _cacheService = cacheService;
//        }
        
//        public async Task<ResultDto<Employee>> CreateAsync(CreateEmployeeDto dto)
//        {
//            _logger.LogInformation("Creating new employee with name: {FullName}, username: {UserName}, station: {StationId}", 
//                dto.FullName, dto.UserName, dto.StationId);

//            using var transaction = await _context.Database.BeginTransactionAsync();

//            try
//            {
//                var employee = new Employee(
//                    dto.FullName,
//                    dto.CardNumber,
//                    dto.StationId,
//                    dto.PhoneNumber,
//                    dto.EmailAddress,
//                    dto.Age,
//                    dto.IdentificationNumber,
//                    dto.Address
//                );
//                await _context.Employees.AddAsync(employee);
//                await _context.SaveChangesAsync();

//                var user = new FuelMasterUser(
//                    dto.UserName,
//                    false,
//                    employee.Id
//                )
//                {
//                    GroupId = dto.GroupId
//                };
//                await _userService.RegisterAsync(user, dto.Password);

//                await transaction.CommitAsync();

//                // Invalidate cache after creating employee
//                await InvalidateEmployeesCacheAsync();
                
//                _logger.LogInformation("Successfully created employee with ID: {Id} and username: {UserName}", 
//                    employee.Id, dto.UserName);

//                return Results.Success(employee);
//            }
//            catch (Exception ex)
//            {
//                await transaction.RollbackAsync();
                
//                _logger.LogError(ex, "Error creating employee with name: {FullName}, username: {UserName}", 
//                    dto.FullName, dto.UserName);

//                return Results.Failure<Employee>(ex.Message);
//            }
//        }

//        public async Task<Employee?> DetailsAsync(int employeeId)
//        {
//            _logger.LogInformation("Getting details for employee with ID: {EmployeeId}", employeeId);

//            var cacheKey = $"Employee_Details_{employeeId}";
//            var cachedEmployee = await _cacheService.GetAsync<Employee>(cacheKey);
            
//            if (cachedEmployee != null)
//            {
//                _logger.LogInformation("Retrieved employee details from cache for ID: {EmployeeId}", employeeId);
//                return cachedEmployee;
//            }

//            _logger.LogInformation("Employee details not in cache, fetching from database for ID: {EmployeeId}", employeeId);

//            var employee = await _context.Employees
//                .Include(x => x.User)
//                .SingleOrDefaultAsync(x => x.Id == employeeId);
            
//            if (employee != null)
//            {
//                await _cacheService.SetAsync(cacheKey, employee);
//                _logger.LogInformation("Cached employee details for ID: {EmployeeId}", employeeId);
//            }

//            return employee;
//        }

//        public async Task<ResultDto<Employee>> EditAsync(int employeeId, EditEmployeeDto dto)
//        {
//            _logger.LogInformation("Editing employee with ID: {EmployeeId}, name: {FullName}, username: {UserName}", 
//                employeeId, dto.FullName, dto.UserName);

//            try
//            {
//                var employee = await _context.Employees
//                    .SingleOrDefaultAsync(x => x.Id == employeeId);
//                if (employee is null)
//                {
//                    _logger.LogWarning("Employee with ID {EmployeeId} not found", employeeId);
//                    return Results.Failure(Resource.EntityNotFound, employee);
//                }

//                employee.FullName = dto.FullName;
//                employee.CardNumber = dto.CardNumber;
//                employee.StationId = dto.StationId;
//                employee.PhoneNumber = dto.PhoneNumber;
//                employee.EmailAddress = dto.EmailAddress;
//                employee.Age = dto.Age;
//                employee.IdentificationNumber = dto.IdentificationNumber;
//                employee.Address = dto.Address;
//                _context.Employees.Update(employee);

//                var user = await _userService.UserManager.Users
//                    .SingleOrDefaultAsync(x => x.EmployeeId == employee.Id);
//                if (user is null)
//                {
//                    _logger.LogWarning("User not found for employee ID: {EmployeeId}", employeeId);
//                    return Results.Failure(Resource.EntityNotFound, employee);
//                }

//                var theUserNameIsUsed = await _userService.UserManager.Users.AnyAsync(x => x.UserName == dto.UserName && x.Id != user.Id);
//                if (theUserNameIsUsed)
//                {
//                    _logger.LogWarning("Username {UserName} is already used", dto.UserName);
//                    return Results.Failure(Resource.UserNameIsUsed, employee);
//                }

//                user.UserName = dto.UserName;
//                user.IsActive = dto.IsActive;
//                user.GroupId = dto.GroupId;

//                var result = await _userService.UpdateAsync(user);
//                await _context.SaveChangesAsync();

//                // Invalidate cache after editing employee
//                await InvalidateEmployeesCacheAsync();
                
//                _logger.LogInformation("Successfully updated employee with ID: {EmployeeId}", employeeId);

//                return Results.Success(employee);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error editing employee with ID: {EmployeeId}", employeeId);
//                return Results.Failure<Employee>(ex.Message);
//            }
//        }

//        public async Task<ResultDto> EditPasswordAsync(int employeeId, EditPasswordDto dto)
//        {
//            _logger.LogInformation("Editing password for employee with ID: {EmployeeId}", employeeId);

//            try
//            {
//                var employee = await _context.Employees
//                    .Include(x => x.User)
//                    .SingleOrDefaultAsync(x => x.Id == employeeId);
//                if (employee is null || employee.User is null)
//                {
//                    _logger.LogWarning("Employee or user not found for ID: {EmployeeId}", employeeId);
//                    return Results.Failure(Resource.EntityNotFound);
//                }

//                await _accountService.EditPasswordAsync(employee.User.Id, dto);
                
//                _logger.LogInformation("Successfully updated password for employee with ID: {EmployeeId}", employeeId);

//                return Results.Success();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error editing password for employee with ID: {EmployeeId}", employeeId);
//                return Results.Failure(ex.Message);
//            }
//        }

//        public async Task<List<Employee>> GetAsync(int? stationId)
//        {
//            _logger.LogInformation("Getting employees for station: {StationId}", stationId);

//            stationId = (await _authorization.TryToGetStationIdAsync()) ?? stationId;
            
//            var cacheKey = $"Employees_Station_{stationId}";
//            var cachedEmployees = await _cacheService.GetAsync<List<Employee>>(cacheKey);
            
//            if (cachedEmployees != null)
//            {
//                _logger.LogInformation("Retrieved {Count} employees from cache for station: {StationId}", 
//                    cachedEmployees.Count, stationId);
//                return cachedEmployees;
//            }

//            _logger.LogInformation("Employees not in cache, fetching from database for station: {StationId}", stationId);
            
//            var employees = await _context.Employees
//                .Where(x => !stationId.HasValue || x.StationId == stationId)
//                .ToListAsync();
            
//            await _cacheService.SetAsync(cacheKey, employees);
            
//            _logger.LogInformation("Cached {Count} employees for station: {StationId}", employees.Count, stationId);

//            return employees;
//        }

//        public async Task<PaginationDto<Employee>> GetPaginationAsync(GetPaginationEmployeeDto dto)
//        {
//            _logger.LogInformation("Getting paginated employees for page {Page}, station: {StationId}, group: {GroupId}, name: {FullName}", 
//                dto.Page, dto.StationId, dto.GroupId, dto.FullName);

//            dto.StationId = (await _authorization.TryToGetStationIdAsync()) ?? dto.StationId;
            
//            // Create cache key based on parameters
//            var cacheKey = $"Employees_Page_{dto.Page}_Station_{dto.StationId}_Group_{dto.GroupId}_Name_{dto.FullName}";
//            var cachedPagination = await _cacheService.GetAsync<PaginationDto<Employee>>(cacheKey);
            
//            if (cachedPagination != null)
//            {
//                _logger.LogInformation("Retrieved paginated employees from cache for page {Page}", dto.Page);
//                return cachedPagination;
//            }

//            _logger.LogInformation("Paginated employees not in cache, fetching from database for page {Page}", dto.Page);
            
//            var pagination = await _context.Employees
//                .Include(x => x.User)
//                .Include(x => x.User!.Group)
//                .Include(x => x.Station)
//                .Where(x => !dto.GroupId.HasValue || x.User!.GroupId == dto.GroupId)
//                .Where(x => dto.FullName == null || x.FullName == dto.FullName)
//                .Where(x => !dto.StationId.HasValue || x.StationId == dto.StationId)
//                .ToPaginationAsync(dto.Page);
            
//            await _cacheService.SetAsync(cacheKey, pagination);
            
//            _logger.LogInformation("Cached paginated employees for page {Page}", dto.Page);

//            return pagination;
//        }

//        private async Task InvalidateEmployeesCacheAsync()
//        {
//            await _cacheService.RemoveByPatternAsync("Employees");
//            _logger.LogInformation("Invalidated all employees cache entries");
//        }
//    }
//}
