using System;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface IFuelTypeRepository : 
    IDefaultQuerableRepository<FuelType>, IRepository<FuelType>, IScopedDependency
{

}
