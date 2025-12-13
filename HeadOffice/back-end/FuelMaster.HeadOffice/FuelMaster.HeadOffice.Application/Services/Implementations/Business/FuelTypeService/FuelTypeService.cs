using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Extensions;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Core;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.FuelTypes;

public class FuelTypeService : IFuelTypeService
{
    private readonly ILogger<FuelTypeService> _logger;
    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEntityCache<FuelType> _fuelTypeCache;
    public FuelTypeService(
        ILogger<FuelTypeService> logger,
        IFuelTypeRepository fuelTypeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEntityCache<FuelType> fuelTypeCache)
    {
        _logger = logger;
        _fuelTypeRepository = fuelTypeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fuelTypeCache = fuelTypeCache;
    }

    public async Task<ResultDto<FuelTypeResult>> CreateAsync(FuelTypeDto dto)
    {
        try
        {
            var fuelType = new FuelType(dto.ArabicName, dto.EnglishName);
            _fuelTypeRepository.Create(fuelType);
            await _unitOfWork.SaveChangesAsync();

            // Update caches incrementally
            await _fuelTypeCache.UpdateCacheAfterCreateAsync(fuelType);

            _logger.LogInformation("Successfully created fuel type with ID: {Id}", fuelType.Id);

            return Result.Success(_mapper.Map<FuelTypeResult>(fuelType));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating fuel type with Arabic name: {ArabicName}, English name: {EnglishName}", 
                dto.ArabicName, dto.EnglishName);
            return Result.Failure<FuelTypeResult>(ex.Message);
        }    
    }

    public async Task<ResultDto> DeleteAsync(int id)
    {
        try
        {
            var fuelTypes = await GetCachedFuelTypesAsync();
            var fuelType = fuelTypes.FirstOrDefault(x => x.Id == id);
            if (fuelType is null)
                return Result.Failure(Resource.EntityNotFound);
                
            _fuelTypeRepository.Delete(fuelType);
            await _unitOfWork.SaveChangesAsync();
            await _fuelTypeCache.UpdateCacheAfterDeleteAsync(id);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting fuel type with ID: {Id}", id);
            return Result.Failure(ex.Message);
        }
    }

    public async Task<FuelTypeResult?> DetailsAsync(int id)
    {
        var fuelTypes = await GetCachedFuelTypesAsync();
        return _mapper.Map<FuelTypeResult?>(fuelTypes.FirstOrDefault(x => x.Id == id));
    }

    public async Task<ResultDto<FuelTypeResult>> UpdateAsync(int id, FuelTypeDto dto)
    {
        var fuelTypes = await GetCachedFuelTypesAsync();
        var fuelType = fuelTypes.FirstOrDefault(x => x.Id == id);
        if (fuelType is null)
            return Result.Failure<FuelTypeResult>(Resource.EntityNotFound);
            
        fuelType.Update(dto.ArabicName, dto.EnglishName);
        _fuelTypeRepository.Update(fuelType);
        await _unitOfWork.SaveChangesAsync();

        await _fuelTypeCache.UpdateCacheAfterEditAsync(fuelType);
        return Result.Success(_mapper.Map<FuelTypeResult>(fuelType));
    }

    public async Task<IEnumerable<FuelTypeResult>> GetAllAsync()
    {
        var fuelTypes = await GetCachedFuelTypesAsync();
        return _mapper.Map<IEnumerable<FuelTypeResult>>(fuelTypes);
    }

    public async Task<List<FuelType>> GetCachedFuelTypesAsync()
    {
        var cachedFuelTypes = await _fuelTypeCache.GetAllEntitiesAsync();
        if (cachedFuelTypes != null)
        {
            return cachedFuelTypes.ToList();
        }
        
        var fuelTypes = await _fuelTypeRepository.GetAllAsync();
        await _fuelTypeCache.SetAllAsync(fuelTypes);
        return fuelTypes.ToList();
    }

    public async Task<PaginationDto<FuelTypeResult>> GetPaginationAsync(int currentPage)
    {
        var fuelTypes = await GetCachedFuelTypesAsync();
        var paginatedData = fuelTypes.ToPagination(currentPage);
        return _mapper.Map<PaginationDto<FuelTypeResult>>(paginatedData);
    }
}
