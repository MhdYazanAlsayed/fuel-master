using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class PumpConfiguration : IEntityTypeConfiguration<Pump>
    {
        public void Configure(EntityTypeBuilder<Pump> builder)
        {
            builder.HasOne(x => x.Station)
                .WithMany()
                .HasForeignKey(x => x.StationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Nozzles)
                .WithOne(x => x.Pump)
                .HasForeignKey(x => x.PumpId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(x => x.Nozzles)
                .HasField("_nozzles")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.StationId)
                .HasColumnName(nameof(Pump.StationId));

            builder.Property(x => x.Number)
                .HasColumnName(nameof(Pump.Number));

            builder.Property(x => x.Manufacturer)
                .HasColumnName(nameof(Pump.Manufacturer));
        }
    }
}


