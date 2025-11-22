using System;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;
using FuelMaster.HeadOffice.Core.Interfaces.Repositories.FuelTypes.Dtos;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;
using FuelMaster.HeadOffice.Core.Models.Dtos;

namespace FuelMaster.HeadOffice.Core.Interfaces.Repositories.FuelTypes;

public interface IFuelTypeRepository : IScopedDependency
{
    Task<IEnumerable<FuelType>> GetAllAsync();
    Task<PaginationDto<FuelType>> GetPaginationAsync(int currentPage);
    Task<ResultDto<FuelType>> CreateAsync(FuelTypeDto dto);
    Task<ResultDto<FuelType>> UpdateAsync(int id, FuelTypeDto dto);
}
