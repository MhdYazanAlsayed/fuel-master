using FuelMaster.Website.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.Website.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }
    public DbSet<PaymentCard> PaymentCards { get; set; }
    public DbSet<BillingHistory> BillingHistories { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantBackup> TenantBackups { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure ApplicationUser
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        // Configure SubscriptionPlan
        builder.Entity<SubscriptionPlan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure UserSubscription
        builder.Entity<UserSubscription>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Plan)
                .WithMany(p => p.UserSubscriptions)
                .HasForeignKey(e => e.PlanId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure PaymentCard
        builder.Entity<PaymentCard>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CardLastFour).HasMaxLength(4).IsRequired();
            entity.Property(e => e.CardBrand).HasMaxLength(50);
            entity.Property(e => e.Token).IsRequired();
            entity.HasOne(e => e.User)
                .WithMany(u => u.PaymentCards)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure BillingHistory
        builder.Entity<BillingHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.HasOne(e => e.UserSubscription)
                .WithMany(s => s.BillingHistories)
                .HasForeignKey(e => e.UserSubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Tenant
        builder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.DatabaseName).HasMaxLength(200);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasOne(e => e.User)
                .WithMany(u => u.Tenants)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Plan)
                .WithMany(p => p.Tenants)
                .HasForeignKey(e => e.PlanId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure TenantBackup
        builder.Entity<TenantBackup>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BackupName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.BackupLocation).HasMaxLength(500);
            entity.HasOne(e => e.Tenant)
                .WithMany(t => t.Backups)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure AuditLog
        builder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Action).HasMaxLength(100).IsRequired();
            entity.Property(e => e.EntityType).HasMaxLength(100).IsRequired();
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.EntityType);
            entity.HasIndex(e => e.EntityId);
        });
    }
}

