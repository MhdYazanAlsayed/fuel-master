using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class ZonePriceHistoryConfiguration : IEntityTypeConfiguration<ZonePriceHistory>
    {
        public void Configure(EntityTypeBuilder<ZonePriceHistory> builder)
        {
            builder.HasOne(x => x.ZonePrice)
                .WithMany(x => x.Histories)
                .HasForeignKey(x => x.ZonePriceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.PriceBeforeChange)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.PriceAfterChange)
                .HasColumnType("decimal(8,2)");
        }
    }
}


