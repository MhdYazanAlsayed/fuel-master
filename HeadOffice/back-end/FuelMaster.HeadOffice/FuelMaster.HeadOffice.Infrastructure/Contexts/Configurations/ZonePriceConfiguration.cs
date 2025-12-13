using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class ZonePriceConfiguration : IEntityTypeConfiguration<ZonePrice>
    {
        public void Configure(EntityTypeBuilder<ZonePrice> builder)
        {
            builder.HasOne(x => x.Zone)
                .WithMany(x => x.Prices)
                .HasForeignKey(x => x.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Price)
                .HasColumnType("decimal(8,2)");

            builder.HasMany(x => x.Histories)
                .WithOne(x => x.ZonePrice)
                .HasForeignKey(x => x.ZonePriceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(x => x.Histories)
                .HasField("_histories")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder
            .HasOne(x => x.FuelType)
            .WithMany()
            .HasForeignKey(x => x.FuelTypeId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.ZoneId)
                .HasColumnName(nameof(ZonePrice.ZoneId));

            builder.Property(x => x.FuelTypeId)
                .HasColumnName(nameof(ZonePrice.FuelTypeId));
        }
    }
}


