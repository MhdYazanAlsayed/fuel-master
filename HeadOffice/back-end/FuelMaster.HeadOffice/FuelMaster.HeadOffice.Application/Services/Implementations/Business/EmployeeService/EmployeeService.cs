using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Extensions;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.EmployeeService;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EmployeeService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserManagerFactory _userManagerFactory;
    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<EmployeeService> logger,
        IUserManagerFactory userManagerFactory)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _userManagerFactory = userManagerFactory;
    }

    public async Task<IEnumerable<EmployeeResult>> GetAllAsync()
    {
        try
        {
            var employees = await _employeeRepository.GetAllAsync(includeRole: true, includeStation: true, includeArea: true, includeCity: true, includeUser: true);
            return _mapper.Map<IEnumerable<EmployeeResult>>(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all employees");
            return Enumerable.Empty<EmployeeResult>();
        }
    }

    public async Task<PaginationDto<EmployeeResult>> GetPaginationAsync(int currentPage)
    {
       
        var (allEmployees, totalCount) = await _employeeRepository.GetPaginationAsync(currentPage, 20, includeRole: true, includeStation: true, includeArea: true, includeCity: true, includeUser: true);
        var mappedData = _mapper.Map<List<EmployeeResult>>(allEmployees);

        return new PaginationDto<EmployeeResult>(mappedData, (int)Math.Ceiling(totalCount / 20m));
    }

    public async Task<ResultDto<EmployeeResult>> CreateAsync(CreateEmployeeDto dto)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            // Create employee entity
            var employee = new Employee(
                dto.FullName,
                dto.CardNumber,
                dto.Scope,
                dto.RoleId,
                dto.StationId,
                dto.AreaId,
                dto.CityId,
                dto.PhoneNumber,
                dto.EmailAddress,
                dto.Age,
                dto.IdentificationNumber,
                dto.Address,
                dto.PTSNumber);

            _employeeRepository.Create(employee);
            await _unitOfWork.SaveChangesAsync();

            // Create user account for the employee
            var userManager = _userManagerFactory.CreateUserManager();
            var user = new FuelMasterUser(dto.UserName, true, employee.Id);
            var createUserResult = await userManager.CreateAsync(user, dto.Password);

            if (!createUserResult.Succeeded)
            {
                _logger.LogError("Error creating user account for employee {EmployeeId}: {Errors}",
                    employee.Id, string.Join(", ", createUserResult.Errors.Select(e => e.Description)));
                
                // Rollback employee creation
                _employeeRepository.Delete(employee);
                await _unitOfWork.SaveChangesAsync();
                
                return Result.Failure<EmployeeResult>(
                    $"Failed to create user account: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
            }

            _logger.LogInformation("Successfully created employee with ID: {Id} and user account", employee.Id);
            await _unitOfWork.CommitTransactionAsync();

            var employeeResult = _mapper.Map<EmployeeResult>(employee);
            return Result.Success(employeeResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee with FullName: {FullName}, CardNumber: {CardNumber}",
                dto.FullName, dto.CardNumber);
            await _unitOfWork.RollbackTransactionAsync();
            return Result.Failure<EmployeeResult>("Error creating employee");
        }
    }

    public async Task<ResultDto<EmployeeResult>> UpdateAsync(int id, UpdateEmployeeDto dto)
    {
        try
        {
            var employee = await _employeeRepository.DetailsAsync(id);
            if (employee is null)
                return Result.Failure<EmployeeResult>(Resource.EntityNotFound);

            employee.Update(
                dto.FullName,
                dto.CardNumber,
                dto.Scope,
                dto.RoleId,
                dto.StationId,
                dto.AreaId,
                dto.CityId,
                dto.PhoneNumber,
                dto.EmailAddress,
                dto.Age,
                dto.IdentificationNumber,
                dto.Address,
                dto.PTSNumber);

            _employeeRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync();

            var updated = _mapper.Map<EmployeeResult>(employee);

            _logger.LogInformation("Successfully updated employee with ID: {Id}", id);

            return Result.Success(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee with ID: {Id}", id);
            return Result.Failure<EmployeeResult>(Resource.EntityNotFound);
        }
    }

    public async Task<EmployeeResult?> DetailsAsync(int id)
    {
        try
        {
            var employees = await _employeeRepository.GetAllAsync(includeRole: true, includeStation: true, includeArea: true, includeCity: true, includeUser: true);
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee is null)
                return null;

            return _mapper.Map<EmployeeResult>(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employee details with ID: {Id}", id);
            return null;
        }
    }
}

