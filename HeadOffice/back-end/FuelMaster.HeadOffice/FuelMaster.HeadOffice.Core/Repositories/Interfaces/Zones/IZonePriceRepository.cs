using System;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;

namespace FuelMaster.HeadOffice.Core.Services.Interfaces.Zones;

public interface IZonePriceRepository : IRepository<ZonePrice>, IScopedDependency
{
}
