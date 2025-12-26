using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.Transactions.TransactionService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.Command.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.Commands;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core.Zones;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Transactions.Processors;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Enums;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Dto;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Results;
using FuelMaster.HeadOffice.Core.Resources;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Transactions.TransactionService;

public class TransactionCommandService : ITransactionCommand
{
    private readonly ITransactionProcessorFactory _transactionProcessorFactory;
    private readonly INozzleService _nozzleService;
    private readonly IZoneService _zoneService;
    private readonly IEmployeeService _employeeService;
    public TransactionCommandService(
    ITransactionProcessorFactory transactionProcessorFactory,
    INozzleService nozzleService,
    IZoneService zoneService,
    IEmployeeService employeeService)
    {
        _transactionProcessorFactory = transactionProcessorFactory;
        _nozzleService = nozzleService;
        _zoneService = zoneService;
        _employeeService = employeeService;
    }

    public async Task<ResultDto<TransactionResult>> CreateAsync (CreateTransactionDto dto)
    {
        var processor = _transactionProcessorFactory.GetProcessor(dto.TransactionType);

        var nozzle = (await _nozzleService.GetCachedNozzlesAsync()).Single(x => x.Id == dto.NozzleId);
        var tank = nozzle.Tank!;
        var stationId = nozzle.Tank!.StationId!;
        
        // Validate nozzle
        var nozzleValidation = await ValidateNozzleAsync(stationId, nozzle.Number);
        if (!nozzleValidation.IsValid)
        {
            return Result.Failure<TransactionResult>("Nozzle exception");
        }

        // Validate zone prices
        var zoneId = tank.Station!.ZoneId!;
        var fuelTypeId = nozzle.FuelTypeId;
        var zonePriceValidation = await ValidateZonePricesAsync(zoneId, fuelTypeId);
        if (!zonePriceValidation.IsValid)
        {
            return Result.Failure<TransactionResult>("Zone price exception");
        }

        // Validate employee
        var employeeValidation = await ValidateEmployeeAsync(dto.EmployeeCardNumber, stationId);
        if (!employeeValidation.IsValid)
        {
            return Result.Failure<TransactionResult>("Employee exception");
        }

        // Validate tank volume
        var tankValidation = ValidateTankVolume(tank, dto.Volume);
        if (!tankValidation.IsValid)
        {
            return Result.Failure<TransactionResult>("Tank volume exception");
        }

        return await processor.ProcessAsync(new ProcessTransactionDto
        {
            UId = dto.UId,
            Nozzle = nozzle,
            PaymentMethod = dto.PaymentMethod,
            Price = dto.Price,
            Amount = dto.Amount,
            Volume = dto.Volume,
            DateTime = dto.DateTime,
            TotalVolume = dto.TotalVolume,
            Employee = employeeValidation.Employee!,
            Tank = tank
        });
    }


    private async Task<NozzleValidationResult> ValidateNozzleAsync(int stationId, int nozzleNumber)
    {
        var nozzles = await _nozzleService.GetCachedNozzlesAsync();
        var nozzle = nozzles.SingleOrDefault(x => x.Number == nozzleNumber && x.Tank!.StationId == stationId);

        if (nozzle is null)
        {
            return new NozzleValidationResult
            {
                IsValid = false,
                Errors = new List<string> { Resource.NozzleNotFound }
            };
        }

        if (nozzle.Status == NozzleStatus.Disabled)
        {
            return new NozzleValidationResult
            {
                IsValid = false,
                Errors = new List<string> { Resource.NozzleDisabled }
            };
        }

        return new NozzleValidationResult
        {
            IsValid = true,
            Nozzle = nozzle
        };
    }

    private async Task<ZonePriceValidationResult> ValidateZonePricesAsync(int zoneId, int fuelTypeId)
    {
        var zonePrices = await _zoneService.GetPricesAsync(zoneId);
        if (zonePrices is null || zonePrices.Count() == 0)
        {
            return new ZonePriceValidationResult
            {
                IsValid = false,
                Errors = new List<string> { Resource.ZonePricesNotFound }
            };
        }

        var zonePrice = zonePrices
            .SingleOrDefault(x => x.FuelTypeId == fuelTypeId);

        if (zonePrice is null)
        {
            return new ZonePriceValidationResult
            {
                IsValid = false,
                Errors = new List<string> { Resource.ZonePricesNotFound }
            };
        }

        return new ZonePriceValidationResult
        {
            IsValid = true,
            ZonePrice = zonePrice
        };
    }

    private async Task<EmployeeValidationResult> ValidateEmployeeAsync(string cardNumber, int stationId)
    {
        var employee = await _employeeService.GetByCardNumberAsync(cardNumber);

        if (employee is null || employee.Station is null || employee.User is null || !employee.User!.IsActive)
        {
            return new EmployeeValidationResult
            {
                IsValid = false,
                Errors = new List<string> { Resource.EmployeeNotFound }
            };
        }

        if (employee.Station.Id != stationId)
        {
            return new EmployeeValidationResult
            {
                IsValid = false,
                Errors = new List<string> { Resource.EmployeeNotInStation }
            };
        }

        return new EmployeeValidationResult
        {
            IsValid = true,
            Employee = employee
        };
    }

    private TankVolumeValidationResult ValidateTankVolume(Tank tank, decimal volume)
    {
        if (tank.CurrentVolume - volume < tank.MinLimit)
        {
            return new TankVolumeValidationResult
            {
                IsValid = false,
                Errors = new List<string> { Resource.TankHasNoFuelEnough }
            };
        }

        return new TankVolumeValidationResult
        {
            IsValid = true
        };
    }

}