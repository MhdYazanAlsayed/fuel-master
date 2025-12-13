using AutoMapper;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Extensions;
using FuelMaster.HeadOffice.Application.Helpers;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.FuelMasterRoleService.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.FuelMasterRoleService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using FuelMaster.HeadOffice.Core.Interfaces;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using FuelMaster.HeadOffice.Core.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.Business.FuelMasterRoleService;

public class FuelMasterRoleService : IFuelMasterRoleService
{
    private readonly IFuelMasterRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<FuelMasterRoleService> _logger;

    public FuelMasterRoleService(
        IFuelMasterRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<FuelMasterRoleService> logger)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<FuelMasterRoleResult>> GetAllAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return _mapper.Map<List<FuelMasterRoleResult>>(roles);
    }

    public Task<PaginationDto<FuelMasterRoleResult>> GetPaginationAsync(int currentPage)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultDto<FuelMasterRoleResult>> CreateAsync(FuelMasterRoleDto dto)
    {
        try
        {
            var role = new FuelMasterRole(dto.ArabicName, dto.EnglishName);

            // Now that the role has an Id, add areas of access
            foreach (var areaOfAccess in dto.AreasOfAccessIds)
            {
                role.AddAreaOfAccess(areaOfAccess);
            }

            _roleRepository.Create(role);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully created role with ID: {Id}", role.Id);

            return Result.Success(_mapper.Map<FuelMasterRoleResult>(role));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating role with Arabic name: {ArabicName}, English name: {EnglishName}",
                dto.ArabicName, dto.EnglishName);
            return Result.Failure<FuelMasterRoleResult>(Resource.EntityNotFound);
        }
    }

    public async Task<ResultDto<FuelMasterRoleResult>> UpdateAsync(int id, FuelMasterRoleDto dto)
    {
        try
        {
            var role = await _roleRepository.DetailsAsync(id);
            if (role is null)
                return Result.Failure<FuelMasterRoleResult>(Resource.EntityNotFound);

            // Update role names
            role.ArabicName = dto.ArabicName;
            role.EnglishName = dto.EnglishName;

            // Clear existing areas of access and add new ones
            var existingAreas = role.AreasOfAccess.ToList();
            foreach (var existingArea in existingAreas)
            {
                role.RemoveAreaOfAccess(existingArea);
            }

            foreach (var areaOfAccess in dto.AreasOfAccessIds)
            {
                role.AddAreaOfAccess(areaOfAccess);
            }

            _roleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(_mapper.Map<FuelMasterRoleResult>(role));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating role with ID: {Id}", id);
            return Result.Failure<FuelMasterRoleResult>(Resource.EntityNotFound);
        }
    }

    public Task<ResultDto> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<FuelMasterRoleResult?> DetailsAsync(int id)
    {
        var role = await _roleRepository.GetAllAsync(includeAreasOfAccess: true);
        
        if (role is null)
            return null;

        return _mapper.Map<FuelMasterRoleResult>(role);
    }
}

