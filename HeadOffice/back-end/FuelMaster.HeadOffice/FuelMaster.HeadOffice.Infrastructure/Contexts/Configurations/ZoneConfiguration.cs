using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class ZoneConfiguration : IEntityTypeConfiguration<Zone>
    {
        public void Configure(EntityTypeBuilder<Zone> builder)
        {
            builder.HasMany(x => x.Prices)
                .WithOne(x => x.Zone)
                .HasForeignKey(x => x.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(x => x.Prices)
                .HasField("_prices")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(x => x.Stations)
                .HasField("_stations")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.ArabicName)
                .HasColumnName(nameof(Zone.ArabicName));

            builder.Property(x => x.EnglishName)
                .HasColumnName(nameof(Zone.EnglishName));
        }
    }
}


