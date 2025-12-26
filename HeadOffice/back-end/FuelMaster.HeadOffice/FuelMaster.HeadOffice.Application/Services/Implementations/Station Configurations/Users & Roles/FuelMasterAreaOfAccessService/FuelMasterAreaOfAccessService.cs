using AutoMapper;
using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.FuelMasterAreaOfAccessService.Results;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business;
using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.FuelMasterAreaOfAccessService;

public class FuelMasterAreaOfAccessService : IFuelMasterAreaOfAccessService
{
    private readonly IFuelMasterAreaOfAccessRepository _areaOfAccessRepository;
    private readonly IEntityCache<FuelMasterAreaOfAccess> _areaOfAccessCache;
    private readonly ILogger<FuelMasterAreaOfAccessService> _logger;
    private readonly IMapper _mapper;

    public FuelMasterAreaOfAccessService(
        IFuelMasterAreaOfAccessRepository areaOfAccessRepository,
        IEntityCache<FuelMasterAreaOfAccess> areaOfAccessCache,
        IMapper mapper,
        ILogger<FuelMasterAreaOfAccessService> logger)
    {
        _areaOfAccessRepository = areaOfAccessRepository;
        _areaOfAccessCache = areaOfAccessCache;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<FuelMasterAreaOfAccessResult>> GetAllAsync()
    {
        var areasOfAccess = await GetCachedAreasOfAccessAsync();

        return _mapper.Map<IEnumerable<FuelMasterAreaOfAccessResult>>(areasOfAccess);
    }

    private async Task<List<FuelMasterAreaOfAccess>> GetCachedAreasOfAccessAsync()
    {
        var cachedAreasOfAccess = await _areaOfAccessCache.GetAllEntitiesAsync();
        if (cachedAreasOfAccess != null)
        {
            return cachedAreasOfAccess.ToList();
        }

        _logger.LogInformation("Areas of access not in cache, fetching from database");

        var areasOfAccess = await _areaOfAccessRepository.GetAllAsync();
        await _areaOfAccessCache.SetAllAsync(areasOfAccess);
        return areasOfAccess.ToList();
    }
}

