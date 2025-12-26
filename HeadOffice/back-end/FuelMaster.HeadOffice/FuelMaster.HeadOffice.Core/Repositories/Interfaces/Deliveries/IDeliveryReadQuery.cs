using FuelMaster.HeadOffice.Core.Entities;

namespace FuelMaster.HeadOffice.Core.Repositories.Interfaces.Deliveries;

public interface IDeliveryReadQuery
{
    IDeliveryReadQuery ApplyFilter(Scope scope, int? cityId, int? areaId, int? stationId);
    Task<(List<Delivery>, int)> GetPaginationAsync(int page, int pageSize, bool includeStation = false, bool includeTank = false);
}