using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Tanents.DTOs;
using FuelMaster.HeadOffice.Application.Services.Implementations.Tanents.Results;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Tenancy;

public interface IMultiTenancyManager : IScopedDependency
{
    Task<ResultDto<DatabaseTanentResult>> CreateDatabaseForTenantAsync(CreateDatabaseForTanentDto createDatabaseForTanentDto);
    Task<ResultDto> DeleteDatabaseForTenantAsync();
    Task<ResultDto> BackupDatabaseForTenantAsync();
}