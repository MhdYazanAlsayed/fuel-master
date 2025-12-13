using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces;

namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories;

public interface ICityRepository : IDefaultQuerableRepository<City>, IRepository<City>, IScopedDependency
{
}