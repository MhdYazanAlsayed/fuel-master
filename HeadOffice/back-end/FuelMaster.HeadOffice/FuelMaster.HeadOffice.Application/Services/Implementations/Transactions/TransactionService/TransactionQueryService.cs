using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.NozzleService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.NozzleService.Results;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.TankService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Authentication;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.Queries.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Zones.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core.Zones;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Enums;
using FuelMaster.HeadOffice.Core.Helpers;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Dto;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Results;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.Extensions.Logging;


namespace FuelMaster.HeadOffice.ApplicationService.Entities;
public class TransactionQueryService : ITransactionQuery
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ISigninService _signinService;
    private readonly IMapper _mapper;
    private readonly ILogger<TransactionQueryService> _logger;
    public TransactionQueryService(
        ITransactionRepository transactionRepository, 
        ISigninService signinService,
        IMapper mapper,
        ILogger<TransactionQueryService> logger)
    {
        _transactionRepository = transactionRepository;
        _signinService = signinService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TransactionResult?> DetailsAsync(long id)
    {
        _logger.LogInformation("Getting details for transaction");

        var result = await _transactionRepository.DetailsAsync(id,
            includeNozzle: true,
            includeEmployee: true,
            includeStation: true,
            includePump: true);

        _logger.LogInformation("Mapping data to TransactionResult");
        var mappedData = _mapper.Map<TransactionResult>(result);

        _logger.LogInformation("Returning transaction details");
        return mappedData;
    }

    public async Task<List<TransactionResult>> GetAllAsync(TransactionsDto dto)
    {
        _logger.LogInformation("Getting all transactions");
        var scope = _signinService.GetCurrentScope() ?? throw new InvalidOperationException("Current scope is not set");
        var cityId = _signinService.GetCurrentCityId();
        var areaId = _signinService.GetCurrentAreaId();
        var stationId = _signinService.GetCurrentStationId();

        var result = await _transactionRepository
        .UseScopeFilter()
        .ApplyFilter(scope, cityId, areaId, stationId)
        .GetAllAsync(
            dto.From, 
            dto.To,
            dto.AreaId, 
            dto.CityId, 
            dto.StationId, 
            dto.NozzleId, 
            dto.PumpId, 
            dto.EmployeeId,
            includeNozzle: true, 
            includeEmployee: true, 
            includeStation: true, 
            includePump: true
        );

        _logger.LogInformation("Mapping data to TransactionResult");
        var mappedData = _mapper.Map<List<TransactionResult>>(result);

        _logger.LogInformation("Returning all transactions");
        return mappedData;
    }

    public async Task<PaginationDto<TransactionResult>> GetPaginationAsync(int page, TransactionPaginationDto dto)
    {
        _logger.LogInformation("Getting pagination for transactions");

        var scope = _signinService.GetCurrentScope() ?? throw new InvalidOperationException("Current scope is not set");
        var cityId = _signinService.GetCurrentCityId();
        var areaId = _signinService.GetCurrentAreaId();
        var stationId = _signinService.GetCurrentStationId();

        var result = await _transactionRepository
        .UseScopeFilter()
        .ApplyFilter(scope, cityId, areaId, stationId)
        .GetPaginationAsync(
            page, 
            30, 
            dto.AreaId, 
            dto.CityId, 
            dto.StationId, 
            dto.NozzleId, 
            dto.PumpId, 
            dto.EmployeeId, 
            dto.From, 
            dto.To, 
            includeNozzle: true, 
            includeEmployee: true, 
            includeStation: true, 
            includePump: true);

        _logger.LogInformation("Mapping data to TransactionResult");
        var mappedData = _mapper.Map<List<TransactionResult>>(result.Data);

        _logger.LogInformation("Getting pagination for transactions");
        return new PaginationDto<TransactionResult>(
            mappedData, 
            (int)Math.Ceiling(result.TotalCount / 30m)
        );
    }
}
