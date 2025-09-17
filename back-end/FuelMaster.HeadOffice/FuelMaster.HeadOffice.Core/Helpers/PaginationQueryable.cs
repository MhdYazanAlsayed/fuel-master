using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Core.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Core.Helpers
{
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
}
