using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.TankService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Deliveries;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Deliveries.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Deliveries.Results;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Deliveries;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.DeliveryService;

public class DeliveryService : IDeliveryService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly ITankService _tankService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeliveryService> _logger;

    public DeliveryService(
        IDeliveryRepository deliveryRepository,
        ITankService tankService,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<DeliveryService> logger)
    {
        _deliveryRepository = deliveryRepository;
        _tankService = tankService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ResultDto<DeliveryResult>> CreateAsync(DeliveryDto dto)
    {
        try
        {
            var tank = await _tankService.DetailsAsync(dto.TankId);
            if (tank == null)
            {
                _logger.LogWarning("Tank with ID {TankId} not found", dto.TankId);
                return Result.Failure<DeliveryResult>("Tank not found");
            }

            await _unitOfWork.BeginTransactionAsync();

            var delivery = Delivery.Create(
                dto.Transport,
                dto.InvoiceNumber,
                dto.PaidVolume,
                dto.RecivedVolume,
                dto.TankId,
                tank.CurrentLevel,
                tank.CurrentVolume);

            _deliveryRepository.Create(delivery);
            await _unitOfWork.SaveChangesAsync();

            // Update tank 
            await _tankService.UpdateAsync(dto.TankId, new EditTankDto
            {
                CurrentLevel = tank.CurrentLevel + dto.RecivedVolume,
                CurrentVolume = tank.CurrentVolume + dto.RecivedVolume,
                HasSensor = tank.HasSensor,
                Capacity = tank.Capacity,
                MaxLimit = tank.MaxLimit,
                MinLimit = tank.MinLimit
            });

            await _unitOfWork.CommitTransactionAsync();
            // Get the delivery with tank included
            delivery = await _deliveryRepository.DetailsAsync(delivery.Id, includeTank: true);
            if (delivery == null)
            {
                return Result.Failure<DeliveryResult>("Failed to retrieve created delivery");
            }


            var deliveryResult = _mapper.Map<DeliveryResult>(delivery);
            return Result.Success(deliveryResult);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Error creating delivery for TankId: {TankId}, InvoiceNumber: {InvoiceNumber}",
                dto.TankId, dto.InvoiceNumber);
            return Result.Failure<DeliveryResult>(ex.Message);
        }
    }

    public async Task<ResultDto> DeleteAsync(int id)
    {
        try
        {
            var delivery = await _deliveryRepository.DetailsAsync(id, includeTank: true);
            if (delivery == null)
            {
                return Result.Failure("Delivery not found");
            }

            await _unitOfWork.BeginTransactionAsync();

            // Update tank
            var tank = await _tankService.DetailsAsync(delivery.TankId);
            if (tank == null)
            {
                return Result.Failure("Tank not found");
            }

            await _tankService.UpdateAsync(delivery.TankId, new EditTankDto
            {
                CurrentLevel = tank.CurrentLevel - delivery.RecivedVolume,
                CurrentVolume = tank.CurrentVolume - delivery.RecivedVolume,
                Capacity = tank.Capacity,
                MaxLimit = tank.MaxLimit,
                MinLimit = tank.MinLimit,
                HasSensor = tank.HasSensor,
            });

            _deliveryRepository.Delete(delivery);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Error deleting delivery with ID: {Id}", id);
            return Result.Failure(ex.Message);
        }
    }

    public async Task<ResultDto<DeliveryResult>> DetailsAsync(int id)
    {
        try
        {
            var delivery = await _deliveryRepository.DetailsAsync(id, includeTank: true);
            if (delivery == null)
            {
                _logger.LogWarning("Delivery with ID {Id} not found", id);
                return Result.Failure<DeliveryResult>("Delivery not found");
            }

            var deliveryResult = _mapper.Map<DeliveryResult>(delivery);
            return Result.Success(deliveryResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting delivery with ID: {Id}", id);
            return Result.Failure<DeliveryResult>(ex.Message);
        }
    }

    public async Task<IEnumerable<DeliveryResult>> GetAllAsync(DeliveryAllDto dto)
    {
        try
        {
            var deliveries = await _deliveryRepository.GetAllAsync(
                from: dto.From,
                to: dto.To,
                cityId: dto.CityId,
                areaId: dto.AreaId,
                stationId: dto.StationId,
                tankId: dto.TankId,
                includeTank: true);

            var deliveryResults = _mapper.Map<List<DeliveryResult>>(deliveries);
            return deliveryResults;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all deliveries");
            return Enumerable.Empty<DeliveryResult>();
        }
    }

    public async Task<PaginationDto<DeliveryResult>> PaginationAsync(DeliveryPaginationDto dto)
    {
        try
        {
            var (deliveries, totalCount) = await _deliveryRepository.GetPaginationAsync(
                page: dto.Page,
                pageSize: Pagination.Length,
                from: dto.From,
                to: dto.To,
                cityId: dto.CityId,
                areaId: dto.AreaId,
                stationId: dto.StationId,
                tankId: dto.TankId,
                includeTank: true);

            var deliveryResults = _mapper.Map<List<DeliveryResult>>(deliveries);
            var pages = (int)Math.Ceiling(totalCount / (decimal)Pagination.Length);
            
            return new PaginationDto<DeliveryResult>(deliveryResults, pages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated deliveries for page: {Page}", dto.Page);
            return new PaginationDto<DeliveryResult>(new List<DeliveryResult>(), 0);
        }
    }
}

