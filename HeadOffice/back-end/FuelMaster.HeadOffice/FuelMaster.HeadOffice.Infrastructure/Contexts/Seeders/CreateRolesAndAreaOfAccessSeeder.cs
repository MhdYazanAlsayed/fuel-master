using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions.Enums;
using FuelMaster.HeadOffice.Infrastructure.Contexts;
using FuelMaster.HeadOffice.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Seeders;

public class CreateRolesAndAreaOfAccessSeeder : ISeeder
{
    private readonly IContextFactory<FuelMasterDbContext> _contextFactory;
    public CreateRolesAndAreaOfAccessSeeder(IContextFactory<FuelMasterDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public int Order => 1;

    public async Task SeedAsync(Guid tenantId)
    {
        var context = await _contextFactory.CreateDbContextForTenantAsync(tenantId);
        
        if (context is null)
            throw new NullReferenceException("Context is null");

        // Check if areas already exist
        if (await context.FuelMasterAreasOfAccess.AnyAsync())
        {
            return;
        }

        // Area of access
        var allAreaOfAccess = new FuelMasterAreaOfAccess(AreaOfAccess.ALL,
                "Full Access", "وصول كامل",
                "Full access to the system", "وصول كامل إلى النظام");
        var areas = new List<FuelMasterAreaOfAccess>()
        {
            allAreaOfAccess,
            new (AreaOfAccess.ConfigurationView,
                "Configuration View", "عرض الإعدادات",
                "View configuration including Stations, Cities, Areas, Tanks, Pumps, Nozzles, Fuel Types, and Zones", 
                "عرض الإعدادات بما في ذلك المحطات والمدن والمناطق والخزانات والمضخات والفوهات وأنواع الوقود والمناطق"),
            
            new (AreaOfAccess.ConfigurationManage,
                "Configuration Manage", "إدارة الإعدادات",
                "Manage configuration including Stations, Cities, Areas, Tanks, Pumps, Nozzles, Fuel Types, and Zones", 
                "إدارة الإعدادات بما في ذلك المحطات والمدن والمناطق والخزانات والمضخات والفوهات وأنواع الوقود والمناطق"),
            
            new (AreaOfAccess.PricingView,
                "Pricing View", "عرض الأسعار",
                "View prices, price histories, and pricing information", 
                "عرض الأسعار وسجلات الأسعار ومعلومات التسعير"),
            
            new (AreaOfAccess.PricingManage,
                "Pricing Manage", "إدارة الأسعار",
                "Manage prices, change prices, and update pricing information", 
                "إدارة الأسعار وتغيير الأسعار وتحديث معلومات التسعير"),
            
            new (AreaOfAccess.DeliveryView,
                "Delivery View", "عرض التسليمات",
                "View deliveries and delivery histories", 
                "عرض التسليمات وسجلات التسليم"),
            
            new (AreaOfAccess.DeliveryManage,
                "Delivery Manage", "إدارة التسليمات",
                "Manage deliveries, change deliveries, and update delivery information", 
                "إدارة التسليمات وتغيير التسليمات وتحديث معلومات التسليم"),
            
            new (AreaOfAccess.EmployeeView,
                "Employee View", "عرض الموظفين",
                "View employees and employee information", 
                "عرض الموظفين ومعلومات الموظفين"),
            
            new (AreaOfAccess.EmployeeManage,
                "Employee Manage", "إدارة الموظفين",
                "Manage employees with users, edit roles and permissions", 
                "إدارة الموظفين مع المستخدمين وتعديل الأدوار والصلاحيات"),
            
            new (AreaOfAccess.OperationView,
                "Operation View", "عرض العمليات",
                "View sales, pump readings, nozzle data, and transaction history", 
                "عرض المبيعات وقراءات الطلمبات وبيانات الفوهات وسجل المعاملات"),
            
            new (AreaOfAccess.OperationManage,
                "Operation Manage", "إدارة العمليات",
                "Manage sales, pump readings, nozzle data, and transaction history", 
                "إدارة المبيعات وقراءات الطلمبات وبيانات الفوهات وسجل المعاملات"),
            
            new (AreaOfAccess.ReportView,
                "Report View", "عرض التقارير",
                "View reports and analytics", 
                "عرض التقارير والتحليلات"),
        };

        await context.FuelMasterAreasOfAccess.AddRangeAsync(areas);
        await context.SaveChangesAsync();


        var adminRole = new FuelMasterRole("مسؤول النظام", "System Administrator");
        adminRole.AddAreaOfAccess(allAreaOfAccess.Id);
        await context.FuelMasterRoles.AddAsync(adminRole);
        await context.SaveChangesAsync();
    }
}