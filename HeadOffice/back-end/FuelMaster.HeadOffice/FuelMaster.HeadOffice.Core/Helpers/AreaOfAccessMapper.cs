using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions.Enums;

namespace FuelMaster.HeadOffice.Core.Helpers;

public static class AreaOfAccessMapper
{
    public static AreaOfAccess Map(string areaOfAccess)
    {
        return areaOfAccess switch
        {
            "ALL" => AreaOfAccess.ALL,
            "ConfigurationView" => AreaOfAccess.ConfigurationView,
            "ConfigurationManage" => AreaOfAccess.ConfigurationManage,
            "PricingView" => AreaOfAccess.PricingView,
            "PricingManage" => AreaOfAccess.PricingManage,
            "DeliveryView" => AreaOfAccess.DeliveryView,
            "DeliveryManage" => AreaOfAccess.DeliveryManage,
            "EmployeeView" => AreaOfAccess.EmployeeView,
            "EmployeeManage" => AreaOfAccess.EmployeeManage,
            "OperationView" => AreaOfAccess.OperationView,
            "OperationManage" => AreaOfAccess.OperationManage,
            "ReportView" => AreaOfAccess.ReportView,
            _ => throw new ArgumentException($"Invalid area of access: {areaOfAccess}"),
        };
    }

    public static string Map(AreaOfAccess areaOfAccess)
    {
        return areaOfAccess switch
        {
            AreaOfAccess.ALL => "ALL",
            AreaOfAccess.ConfigurationView => "ConfigurationView",
            AreaOfAccess.ConfigurationManage => "ConfigurationManage",
            AreaOfAccess.PricingView => "PricingView",
            AreaOfAccess.PricingManage => "PricingManage",
            AreaOfAccess.DeliveryView => "DeliveryView",
            AreaOfAccess.DeliveryManage => "DeliveryManage",
            AreaOfAccess.EmployeeView => "EmployeeView",
            AreaOfAccess.EmployeeManage => "EmployeeManage",
            AreaOfAccess.OperationView => "OperationView",
            AreaOfAccess.OperationManage => "OperationManage",
            AreaOfAccess.ReportView => "ReportView",
            _ => throw new ArgumentException($"Invalid area of access: {areaOfAccess}"),
        };
    }
}