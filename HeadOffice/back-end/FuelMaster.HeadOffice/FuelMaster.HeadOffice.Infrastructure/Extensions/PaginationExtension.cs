using FuelMaster.HeadOffice.Application.DTOs;
using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Extensions;

public static class PaginationExtension
{
    public static async Task<PaginationDto<T>> ToPaginationAsync<T>(this IQueryable<T> query , int page) where T : Base
    {
        var count = await query.CountAsync();

        var pages = (int)Math.Ceiling(Convert.ToDecimal(count) / Convert.ToDecimal(20));

        var result = await query
            .Skip(20 * (page - 1))
            .Take(20)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return new(result, pages);
    }
}