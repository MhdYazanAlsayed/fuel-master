using FuelMaster.HeadOffice.Core.Contracts.Authentication;
using FuelMaster.HeadOffice.Core.Contracts.Database;
using FuelMaster.HeadOffice.Core.Contracts.Entities;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using FuelMaster.HeadOffice.Core.Models.Requests.Accounts;
using FuelMaster.HeadOffice.Core.Models.Requests.Employees;
using FuelMaster.HeadOffice.Core.Resources;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.ApplicationService.Entities
{
    public class EmployeeService : IEmployeeService
    {
        private readonly FuelMasterDbContext _context;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;       
        private readonly IAuthorization _authorization;

        public EmployeeService(
            IContextFactory<FuelMasterDbContext> contextFactory, 
            IUserService userService, 
            IAccountService accountService, 
            IAuthorization authorization)
        {
            _context = contextFactory.CurrentContext;
            _userService = userService;
            _accountService = accountService;
            _authorization = authorization;
        }
        
        public async Task<ResultDto<Employee>> CreateAsync(CreateEmployeeDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var employee = new Employee(
                    dto.FullName,
                    dto.CardNumber,
                    dto.StationId,
                    dto.PhoneNumber,
                    dto.EmailAddress,
                    dto.Age,
                    dto.IdentificationNumber,
                    dto.Address
                );
                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                var user = new FuelMasterUser(
                    dto.UserName,
                    false,
                    employee.Id
                )
                {
                    GroupId = dto.GroupId
                };
                await _userService.RegisterAsync(user, dto.Password);

                await transaction.CommitAsync();

                return Results.Success(employee);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return Results.Failure<Employee>(ex.Message);
            }
        }

        public async Task<Employee?> DetailsAsync(int employeeId)
        {
            return
                await _context.Employees
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == employeeId);
        }

        public async Task<ResultDto<Employee>> EditAsync(int employeeId, EditEmployeeDto dto)
        {
            var employee = await _context.Employees
                .SingleOrDefaultAsync(x => x.Id == employeeId);
            if (employee is null)
                return Results.Failure(Resource.EntityNotFound, employee);

            employee.FullName = dto.FullName;
            employee.CardNumber = dto.CardNumber;
            employee.StationId = dto.StationId;
            employee.PhoneNumber = dto.PhoneNumber;
            employee.EmailAddress = dto.EmailAddress;
            employee.Age = dto.Age;
            employee.IdentificationNumber = dto.IdentificationNumber;
            employee.Address = dto.Address;
            _context.Employees.Update(employee);

            var user = await _userService.UserManager.Users
                .SingleOrDefaultAsync(x => x.EmployeeId == employee.Id);
            if (user is null)
                return Results.Failure(Resource.EntityNotFound, employee);

            var theUserNameIsUsed = await _userService.UserManager.Users.AnyAsync(x => x.UserName == dto.UserName && x.Id != user.Id);
            if (theUserNameIsUsed)
                return Results.Failure(Resource.UserNameIsUsed, employee);

            user.UserName = dto.UserName;
            user.IsActive = dto.IsActive;
            user.GroupId = dto.GroupId;

            var result = await _userService.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return Results.Success(employee);
        }

        public async Task<ResultDto> EditPasswordAsync(int employeeId, EditPasswordDto dto)
        {
            var employee = await _context.Employees
                .Include(x => x.User)
                .SingleOrDefaultAsync(x => x.Id == employeeId);
            if (employee is null || employee.User is null)
                return Results.Failure(Resource.EntityNotFound);

            await _accountService.EditPasswordAsync(employee.User.Id, dto);

            return Results.Success();
        }

        public async Task<List<Employee>> GetAsync(int? stationId)
        {
            stationId = (await _authorization.TryToGetStationIdAsync()) ?? stationId;
            
            return await _context.Employees
                .Where(x => !stationId.HasValue || x.StationId == stationId)
                .ToListAsync();
        }

        public async Task<PaginationDto<Employee>> GetPaginationAsync(GetPaginationEmployeeDto dto)
        {
            dto.StationId = (await _authorization.TryToGetStationIdAsync()) ?? dto.StationId;
            
            return await _context.Employees
                .Include(x => x.User)
                .Include(x => x.User!.Group)
                .Include(x => x.Station)
                .Where(x => !dto.GroupId.HasValue || x.User!.GroupId == dto.GroupId)
                .Where(x => dto.FullName == null || x.FullName == dto.FullName)
                .Where(x => !dto.StationId.HasValue || x.StationId == dto.StationId)
                .ToPaginationAsync(dto.Page);
        }
    }
}
