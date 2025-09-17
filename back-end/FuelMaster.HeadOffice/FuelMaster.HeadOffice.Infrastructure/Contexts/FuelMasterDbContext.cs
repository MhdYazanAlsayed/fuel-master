using FuelMaster.HeadOffice.Core.Entities;
using FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts
{
    public class FuelMasterDbContext : IdentityDbContext<FuelMasterUser>
    {
        public FuelMasterDbContext(DbContextOptions<FuelMasterDbContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new NozzleConfiguration());

            #region Relations

            builder.Entity<Permission>()
                .HasOne(x => x.FuelMasterGroup)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.FuelMasterGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FuelMasterUser>()
                .HasOne(x => x.Group)
                .WithMany()
                .HasForeignKey(x => x.GroupId);

            builder.Entity<Station>()
                .HasOne(x => x.City)
                .WithMany()
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Station>()
                .HasOne(x => x.Zone)
                .WithMany(x => x.Stations)
                .HasForeignKey(x => x.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Delivery>()
                .HasOne(x => x.Tank)
                .WithMany()
                .HasForeignKey(x => x.TankId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Nozzle>()
                .HasOne(x => x.Tank)
                .WithMany(x => x.Nozzles)
                .HasForeignKey(x => x.TankId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Nozzle>()
                .HasOne(x => x.Pump)
                .WithMany(x => x.Nozzles)
                .HasForeignKey(x => x.PumpId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<NozzleHistory>()
                .HasOne(x => x.Nozzle)
                .WithMany()
                .HasForeignKey(x => x.NozzleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<NozzleHistory>()
                .Property(x => x.Volume)
                .HasColumnType("decimal(8,2)");
            
            builder.Entity<NozzleHistory>()
                .Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<NozzleHistory>()
                .HasOne(x => x.Station)
                .WithMany()
                .HasForeignKey(x => x.StationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<NozzleHistory>()
                .HasOne(x => x.Tank)
                .WithMany()
                .HasForeignKey(x => x.TankId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Pump>()
                .HasOne(x => x.Station)
                .WithMany()
                .HasForeignKey(x => x.StationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Tank>()
                .HasOne(x => x.Station)
                .WithMany(x => x.Tanks)
                .HasForeignKey(x => x.StationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TankTransaction>()
                .HasOne(x => x.Tank)
                .WithMany()
                .HasForeignKey(x => x.TankId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasOne(x => x.Nozzle)
                .WithMany()
                .HasForeignKey(x => x.NozzleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ZonePrice>()
                .HasOne(x => x.Zone)
                .WithMany(x => x.Prices)
                .HasForeignKey(x => x.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ZonePriceHistory>()
                .HasOne(x => x.ZonePrice)
                .WithMany(x => x.Histories)
                .HasForeignKey(x => x.ZonePriceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Employee>()
                .HasOne(x => x.User)
                .WithOne(x => x.Employee)
                .HasForeignKey<FuelMasterUser>(x => x.EmployeeId)
                .IsRequired(false);

            builder.Entity<Employee>()
                .HasOne(x => x.Station)
                .WithMany()
                .HasForeignKey(x => x.StationId)
                .OnDelete(DeleteBehavior.Restrict);


            #endregion

            #region Types Of Decmial Properties 

            builder.Entity<Delivery>()
                .Property(x => x.PaidVolume)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Delivery>()
                .Property(x => x.RecivedVolume)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Delivery>()
                .Property(x => x.TankOldLevel)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Delivery>()
                .Property(x => x.TankNewLevel)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Delivery>()
                .Property(x => x.TankOldVolume)
                .HasColumnType("decimal(8,2)");

                builder.Entity<Delivery>()
                .Property(x => x.TankNewVolume)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Delivery>()
                .Property(x => x.GL)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Nozzle>()
                .Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Nozzle>()
                .Property(x => x.Volume)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Nozzle>()
                .Property(x => x.Totalizer)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Nozzle>()
                .Property(x => x.Price)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Tank>()
                .Property(x => x.Capacity)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Tank>()
                .Property(x => x.MaxLimit)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Tank>()
                .Property(x => x.MinLimit)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Tank>()
                .Property(x => x.CurrentLevel)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Tank>()
                .Property(x => x.CurrentVolume)
                .HasColumnType("decimal(8,2)");

            builder.Entity<TankTransaction>()
                .Property(x => x.CurrentVolume)
                .HasColumnType("decimal(8,2)");

            builder.Entity<TankTransaction>()
                .Property(x => x.CurrentLevel)
                .HasColumnType("decimal(8,2)");

            // Transaction 

            builder.Entity<Transaction>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Transaction>()
                .Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Transaction>()
                .Property(x => x.Volume)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Transaction>()
                .Property(x => x.Totalizer)
                .HasColumnType("decimal(8,2)");

            builder.Entity<Transaction>()
                .Property(x => x.TotalizerAfter)
                .HasColumnType("decimal(8,2)");

            builder.Entity<ZonePrice>()
                .Property(x => x.Price)
                .HasColumnType("decimal(8,2)");

            builder.Entity<ZonePriceHistory>()
                .Property(x => x.PriceBeforeChange)
                .HasColumnType("decimal(8,2)");

            #endregion
        }

        public DbSet<Station> Stations { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<FuelMasterGroup> FuelMasterGroup { get; set; }
        public DbSet<Nozzle> Nozzles { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Pump> Pumps { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        public DbSet<TankTransaction> TankTransactions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<ZonePrice> ZonePrices { get; set; }
        public DbSet<ZonePriceHistory> ZonePriceHistory { get; set; }
        public DbSet<NozzleHistory> NozzleHistories { get; set; }
    }
}
