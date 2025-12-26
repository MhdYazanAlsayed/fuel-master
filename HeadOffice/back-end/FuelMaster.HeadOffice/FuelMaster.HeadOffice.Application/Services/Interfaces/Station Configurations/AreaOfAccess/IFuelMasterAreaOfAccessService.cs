using FuelMaster.HeadOffice.Application.Services.Implementations.StationConfigurations.FuelMasterAreaOfAccessService.Results;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Business;

public interface IFuelMasterAreaOfAccessService : IScopedDependency
{
    Task<IEnumerable<FuelMasterAreaOfAccessResult>> GetAllAsync();
}

