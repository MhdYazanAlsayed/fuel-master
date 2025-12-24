using FuelMaster.HeadOffice.Application.DTOs;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Core;

public interface IBusinessService<TDto, TResult> where TDto : class where TResult : class
{
    Task<ResultDto<TResult>> CreateAsync(TDto dto);
    Task<ResultDto<TResult>> UpdateAsync(int id, TDto dto);
    Task<ResultDto> DeleteAsync(int id);
    Task<TResult?> DetailsAsync(int id);
}

public interface IBusinessService<TCreateDto, TEditDto, TResult> where TCreateDto : class where TEditDto : class where TResult : class
{
    Task<ResultDto<TResult>> CreateAsync(TCreateDto dto);
    Task<ResultDto<TResult>> UpdateAsync(int id, TEditDto dto);
    Task<ResultDto> DeleteAsync(int id);
    Task<TResult?> DetailsAsync(int id);
}

public interface IDefaultBusinessQueryService<TResult> where TResult : class
{
    Task<IEnumerable<TResult>> GetAllAsync();
    Task<PaginationDto<TResult>> GetPaginationAsync(int currentPage);
}