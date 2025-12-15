using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions.Enums;

namespace FuelMaster.HeadOffice.Core.Helpers;

public static class AreaOfAccessMapper
{
    public static AreaOfAccess Map(string areaOfAccess)
    {
        return areaOfAccess switch
        {
            "ALL" => AreaOfAccess.ALL,
            "System.Configuration.View" => AreaOfAccess.ConfigurationView,
            "System.Configuration.Manage" => AreaOfAccess.ConfigurationManage,
            "System.Pricing.View" => AreaOfAccess.PricingView,
            "System.Pricing.Manage" => AreaOfAccess.PricingManage,
            "System.Delivery.View" => AreaOfAccess.DeliveryView,
            "System.Delivery.Manage" => AreaOfAccess.DeliveryManage,
            "System.Employee.View" => AreaOfAccess.EmployeeView,
            "System.Employee.Manage" => AreaOfAccess.EmployeeManage,
            "System.Operation.View" => AreaOfAccess.OperationView,
            "System.Operation.Manage" => AreaOfAccess.OperationManage,
            "System.Report.View" => AreaOfAccess.ReportView,
            _ => throw new ArgumentException($"Invalid area of access: {areaOfAccess}"),
        };
    }

    public static string Map(AreaOfAccess areaOfAccess)
    {
        return areaOfAccess switch
        {
            AreaOfAccess.ALL => "ALL",
            AreaOfAccess.ConfigurationView => "System.Configuration.View",
            AreaOfAccess.ConfigurationManage => "System.Configuration.Manage",
            AreaOfAccess.PricingView => "System.Pricing.View",
            AreaOfAccess.PricingManage => "System.Pricing.Manage",
            AreaOfAccess.DeliveryView => "System.Delivery.View",
            AreaOfAccess.DeliveryManage => "System.Delivery.Manage",
            AreaOfAccess.EmployeeView => "System.Employee.View",
            AreaOfAccess.EmployeeManage => "System.Employee.Manage",
            AreaOfAccess.OperationView => "System.Operation.View",
            AreaOfAccess.OperationManage => "System.Operation.Manage",
            AreaOfAccess.ReportView => "System.Report.View",
            _ => throw new ArgumentException($"Invalid area of access: {areaOfAccess}"),
        };
    }
}