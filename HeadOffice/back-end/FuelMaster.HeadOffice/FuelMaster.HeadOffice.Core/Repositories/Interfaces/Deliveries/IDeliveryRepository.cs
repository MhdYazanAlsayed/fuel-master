using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories;
using FuelMaster.HeadOffice.Core.Repositories.Interfaces.Deliveries;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces;

public interface IDeliveryRepository : IRepository<Delivery>, IScopedDependency
{
    Task<(List<Delivery>, int)> GetPaginationAsync(
        int page, 
        int pageSize,
        DateTime? from = null,
        DateTime? to = null,
        int? cityId = null,
        int? areaId = null,
        int? stationId = null,
        int? tankId = null,
        bool includeStation = false, 
        bool includeTank = false);

    Task<List<Delivery>> GetAllAsync(
        DateTime from,
        DateTime to,
        int? cityId = null,
        int? areaId = null,
        int? stationId = null,
        int? tankId = null,
        bool includeStation = false, 
        bool includeTank = false);
    Task<Delivery?> DetailsAsync(int id, bool includeStation = false, bool includeTank = false);
    IDeliveryReadQuery UseScopeFilter();
}

