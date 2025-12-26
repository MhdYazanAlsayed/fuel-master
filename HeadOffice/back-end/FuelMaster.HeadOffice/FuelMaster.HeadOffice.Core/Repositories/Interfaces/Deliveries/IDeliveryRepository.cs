using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Deliveries;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface IDeliveryRepository : IRepository<Delivery>, IScopedDependency
{
    Task<(List<Delivery>, int)> GetPaginationAsync(int page, int pageSize,bool includeStation = false ,bool includeTank = false);
    Task<Delivery?> DetailsAsync(int id, bool includeStation = false, bool includeTank = false);
    IDeliveryReadQuery AsReadQuery();
}

