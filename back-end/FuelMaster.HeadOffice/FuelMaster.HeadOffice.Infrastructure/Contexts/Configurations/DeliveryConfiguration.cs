using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.HasOne(x => x.Tank)
                .WithMany()
                .HasForeignKey(x => x.TankId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.PaidVolume)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.RecivedVolume)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.TankOldLevel)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.TankNewLevel)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.TankOldVolume)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.TankNewVolume)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.GL)
                .HasColumnType("decimal(8,2)");
        }
    }
}


