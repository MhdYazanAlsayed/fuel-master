using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.QueryFilters;

public static class DeliveryQueryFilter
{
    public static void ApplyFilterAsync(this EntityTypeBuilder<Delivery> builder, Scope scope, int? cityId, int? areaId, int? stationId)
    {
    }
}