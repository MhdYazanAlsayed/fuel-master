using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Entities;

namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories;

public interface ICityRepository : IRepository<City>, IScopedDependency
{
}