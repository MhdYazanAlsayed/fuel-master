using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.NozzleService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.TankService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Transactions.DTOs;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Interfaces.Services.Transactions.Results;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Transactions.ManualProcessor;

public class TransactionManualProcessor : ITransactionProcessor
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionManualProcessor> _logger;
    private readonly ITankService _tankService;
    private readonly INozzleService _nozzleService;
    private readonly IMapper _mapper;

    public TransactionManualProcessor(
        ITransactionRepository transactionRepository,
        ITankService tankService,
        INozzleService nozzleService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<TransactionManualProcessor> logger
    )
    {
        _transactionRepository = transactionRepository;
        _tankService = tankService;
        _nozzleService = nozzleService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ResultDto<TransactionResult>> ProcessAsync(ProcessTransactionDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try 
        {
            var _transaction = new Transaction(
                Guid.NewGuid().ToString(),
                dto.PaymentMethod,
                dto.Price,
                dto.Nozzle.Id,
                dto.Price * dto.Volume,
                dto.Volume,
                dto.Employee.Id,
                dto.Nozzle.Totalizer,
                dto.Nozzle.Totalizer + dto.Volume,
                dto.Tank!.StationId,
                DateTime.Now
            );
            _transactionRepository.Create(_transaction);

            // ! Future problem, Services will update the cache.
            // ! We need to deal with that updates in failure cases.
            await UpdateNozzleAsync(dto.Nozzle, dto.Volume, dto.Price);
            await UpdateTankAsync(dto.Tank, dto.Volume, dto.Price);

            await _unitOfWork.CommitTransactionAsync();

            var mappedTransaction = _mapper.Map<TransactionResult>(_transaction);
            return Result.Success(mappedTransaction);
        }
        catch 
        {
            await _unitOfWork.RollbackTransactionAsync();

            // TODO : Rollback the cache updates
            return Result.Failure<TransactionResult>(Resource.SthWentWrong);
        }    
    }

    private async Task UpdateTankAsync(Tank tank, decimal volume, decimal price)
    {
        _logger.LogInformation("Updating tank {TankId} with volume {Volume} and price {Price}", tank.Id, volume, price);

        await _tankService.UpdateAsync(tank.Id, new EditTankDto
        {
            CurrentVolume = tank.CurrentVolume - volume,
            CurrentLevel = tank.CurrentLevel - volume, // TODO : Calculate the current level based on the volume
            HasSensor = tank.HasSensor,
            MaxLimit = tank.MaxLimit,
            MinLimit = tank.MinLimit,
            Capacity = tank.Capacity
        });

        _logger.LogInformation("Tank {TankId} updated successfully", tank.Id);
    }

    private async Task UpdateNozzleAsync(Nozzle nozzle, decimal volume, decimal price)
    {
        _logger.LogInformation("Updating nozzle {NozzleId} with volume {Volume} and price {Price}", nozzle.Id, volume, price);
        
        await _nozzleService.UpdateAsync(nozzle.Id, new UpdateNozzleDto
        {
            Amount = nozzle.Amount + price * volume,
            Volume = nozzle.Volume + volume,
            Totalizer = nozzle.Totalizer + volume,
            Number = nozzle.Number,
            ReaderNumber = nozzle.ReaderNumber
        });

        _logger.LogInformation("Nozzle {NozzleId} updated successfully", nozzle.Id);
    }

}