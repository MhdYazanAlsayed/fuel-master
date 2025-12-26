using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Deliveries.DTOs;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Deliveries.Results;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Deliveries;

public interface IDeliveryService : IScopedDependency
{
    Task<ResultDto> DeleteAsync(int id);
    Task<ResultDto<DeliveryResult>> CreateAsync(DeliveryDto dto);
    Task<ResultDto<DeliveryResult>> DetailsAsync(int id);
    Task<IEnumerable<DeliveryResult>> GetAllAsync(DeliveryAllDto dto);
    Task<PaginationDto<DeliveryResult>> PaginationAsync(DeliveryPaginationDto dto);
}