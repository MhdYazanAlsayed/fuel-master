using System;
using FuelMaster.HeadOffice.Core.Configurations.Statics;
using FuelMaster.HeadOffice.Core.Models.Dtos;

namespace FuelMaster.HeadOffice.Core.Extenssions;

public static class PaginationExtension
{
    public static PaginationDto<T> ToPagination<T>(this IEnumerable<T> source, int currentPage)
    {
        var list = source.ToList();
        var totalCount = list.Count;
        var pages = (int)Math.Ceiling(Convert.ToDecimal(totalCount) / Convert.ToDecimal(Pagination.Length));

        var pageData = list
            .Skip(Pagination.Length * (currentPage - 1))
            .Take(Pagination.Length)
            .ToList();

        return new PaginationDto<T>(pageData, pages);
    }
}
