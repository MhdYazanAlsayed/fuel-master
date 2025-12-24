using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.QueryFilters;

public static class EmployeeQueryFilter 
{ 
    public static void ApplyFilterAsync(this EntityTypeBuilder<Employee> builder, Scope scope, int? cityId, int? areaId, int? stationId)
    {
        if (scope == Scope.ALL)
            return;

        if (scope == Scope.City)
        {
            builder.HasQueryFilter(
                x => 
                x.Scope == Scope.Self || x.Scope == Scope.Station && x.Station!.CityId == cityId || 
                (x.Scope == Scope.Area && x.Area!.CityId == cityId)
            );
        }
        else if (scope == Scope.Area)
        {
            builder.HasQueryFilter(
                x => 
                x.Scope == Scope.Self || x.Scope == Scope.Station && x.Station!.AreaId == areaId 
            );
        }
        else if (scope == Scope.Station || scope == Scope.Self)
        {
            // Don't return any employee
            builder.HasQueryFilter(
                x => false
            );
        }
    }
}