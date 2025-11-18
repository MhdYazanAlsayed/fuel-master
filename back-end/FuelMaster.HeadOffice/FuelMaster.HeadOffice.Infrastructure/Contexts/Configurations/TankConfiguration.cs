using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class TankConfiguration : IEntityTypeConfiguration<Tank>
    {
        public void Configure(EntityTypeBuilder<Tank> builder)
        {
            builder.HasOne(x => x.Station)
                .WithMany(x => x.Tanks)
                .HasForeignKey(x => x.StationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Nozzles)
                .WithOne(x => x.Tank)
                .HasForeignKey(x => x.TankId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.FuelType)
            .WithMany()
            .HasForeignKey(x => x.FuelTypeId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(x => x.Nozzles)
                .HasField("_nozzles")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.Capacity)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.MaxLimit)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.MinLimit)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.CurrentLevel)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.CurrentVolume)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.Number)
                .HasColumnName(nameof(Tank.Number));

            builder.Property(x => x.StationId)
                .HasColumnName(nameof(Tank.StationId));

            builder.Property(x => x.FuelTypeId)
                .HasColumnName(nameof(Tank.FuelTypeId));
        }
    }
}


