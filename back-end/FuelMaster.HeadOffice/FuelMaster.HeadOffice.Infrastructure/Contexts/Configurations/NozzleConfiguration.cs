using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class NozzleConfiguration : IEntityTypeConfiguration<Nozzle>
    {
        public void Configure(EntityTypeBuilder<Nozzle> builder)
        {
            builder.HasOne(x => x.Tank)
                .WithMany(x => x.Nozzles)
                .HasForeignKey(x => x.TankId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Pump)
                .WithMany(x => x.Nozzles)
                .HasForeignKey(x => x.PumpId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(x => x.FuelType)
            .WithMany()
            .HasForeignKey(x => x.FuelTypeId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Volume)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.Totalizer)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.Price)
                .HasColumnType("decimal(8,2)");
        }
    }
}
