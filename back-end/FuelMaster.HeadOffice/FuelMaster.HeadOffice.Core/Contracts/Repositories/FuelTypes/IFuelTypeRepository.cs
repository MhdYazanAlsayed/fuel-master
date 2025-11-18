using System;
using FuelMaster.HeadOffice.Core.Contracts.Markers;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.FuelTypes.Dtos;
using FuelMaster.HeadOffice.Core.Entities.Configs.FuelTypes;
using FuelMaster.HeadOffice.Core.Models.Dtos;

namespace FuelMaster.HeadOffice.Core.Contracts.Repositories.FuelTypes;

public interface IFuelTypeRepository : IScopedDependency
{
    Task<IEnumerable<FuelType>> GetAllAsync();
    Task<PaginationDto<FuelType>> GetPaginationAsync(int currentPage);
    Task<ResultDto<FuelType>> CreateAsync(FuelTypeDto dto);
    Task<ResultDto<FuelType>> UpdateAsync(int id, FuelTypeDto dto);
}
