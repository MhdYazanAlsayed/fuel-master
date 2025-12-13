using FuelMaster.Website.Core.Entities;
using FuelMaster.Website.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.Website.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed Subscription Plans
        if (!await context.SubscriptionPlans.AnyAsync())
        {
            var plans = new List<SubscriptionPlan>
            {
                new SubscriptionPlan
                {
                    Id = Guid.NewGuid(),
                    Name = "Free Plan",
                    Description = "Free plan for testing and evaluation. Perfect for getting started with FuelMaster.",
                    Price = 0,
                    BillingCycle = BillingCycle.Monthly,
                    IsFree = true,
                    Features = "{\"maxUsers\": 5, \"maxStations\": 1, \"support\": \"community\"}",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new SubscriptionPlan
                {
                    Id = Guid.NewGuid(),
                    Name = "Starter Plan",
                    Description = "Perfect for small businesses getting started with fuel management.",
                    Price = 29.99m,
                    BillingCycle = BillingCycle.Monthly,
                    IsFree = false,
                    Features = "{\"maxUsers\": 25, \"maxStations\": 5, \"support\": \"email\", \"backups\": \"weekly\"}",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new SubscriptionPlan
                {
                    Id = Guid.NewGuid(),
                    Name = "Professional Plan",
                    Description = "For growing businesses that need advanced features and priority support.",
                    Price = 79.99m,
                    BillingCycle = BillingCycle.Monthly,
                    IsFree = false,
                    Features = "{\"maxUsers\": 100, \"maxStations\": 20, \"support\": \"priority\", \"backups\": \"daily\", \"apiAccess\": true}",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new SubscriptionPlan
                {
                    Id = Guid.NewGuid(),
                    Name = "Enterprise Plan",
                    Description = "For large organizations with unlimited users and dedicated support.",
                    Price = 199.99m,
                    BillingCycle = BillingCycle.Monthly,
                    IsFree = false,
                    Features = "{\"maxUsers\": -1, \"maxStations\": -1, \"support\": \"dedicated\", \"backups\": \"real-time\", \"apiAccess\": true, \"customIntegration\": true}",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await context.SubscriptionPlans.AddRangeAsync(plans);
            await context.SaveChangesAsync();
        }
    }
}

