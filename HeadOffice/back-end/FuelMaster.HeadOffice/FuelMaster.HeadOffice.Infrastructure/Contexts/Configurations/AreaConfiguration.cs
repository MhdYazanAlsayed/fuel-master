using FuelMaster.HeadOffice.Core.Entities.Configs.Area;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class AreaConfiguration : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.HasMany(x => x.Stations)
                .WithOne(x => x.Area)
                .HasForeignKey(x => x.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(x => x.Stations)
                .HasField("_stations")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.ArabicName)
                .HasMaxLength(100)
                .HasColumnName(nameof(Area.ArabicName));

            builder.Property(x => x.EnglishName)
                .HasMaxLength(100)
                .HasColumnName(nameof(Area.EnglishName));
        }
    }
}

