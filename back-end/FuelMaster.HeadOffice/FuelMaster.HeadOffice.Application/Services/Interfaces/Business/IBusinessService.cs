using System;
using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Core.Interfaces.Markers;

namespace FuelMaster.HeadOffice.Application.Services.Interfaces.Core;

public interface IBusinessService<TDto, TResult> where TDto : class where TResult : class
{
    Task<PaginationDto<TResult>> GetPaginationAsync(int currentPage);
    Task<IEnumerable<TResult>> GetAllAsync();
    Task<ResultDto<TResult>> CreateAsync(TDto dto);
    Task<ResultDto<TResult>> EditAsync(int id, TDto dto);
    Task<ResultDto> DeleteAsync(int id);
    Task<TResult?> DetailsAsync(int id);
}

public interface IBusinessService<TCreateDto, TEditDto, TResult> where TCreateDto : class where TEditDto : class where TResult : class
{
    Task<PaginationDto<TResult>> GetPaginationAsync(int currentPage);
    Task<IEnumerable<TResult>> GetAllAsync();
    Task<ResultDto<TResult>> CreateAsync(TCreateDto dto);
    Task<ResultDto<TResult>> EditAsync(int id, TEditDto dto);
    Task<ResultDto> DeleteAsync(int id);
    Task<TResult?> DetailsAsync(int id);
}