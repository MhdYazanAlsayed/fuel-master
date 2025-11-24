using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.Delivery.Dtos;
using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;

namespace FuelMaster.HeadOffice.Core.Interfaces.Entities
{
    public interface IDeliveryRepository : IScopedDependency
    {
        Task<PaginationDto<Delivery>> GetPaginationAsync(GetDeliveriesPaginationDto dto); 
        Task<ResultDto<Delivery>> CreateAsync(DeliveryDto dto);
        Task<Delivery?> DetailsAsync(int id);
        Task<ResultDto> DeleteAsync(int id);
    }

}
