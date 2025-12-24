using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class StationConfiguration : IEntityTypeConfiguration<Station>
    {
        public void Configure(EntityTypeBuilder<Station> builder)
        {
            builder.HasOne(x => x.City)
                .WithMany()
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Zone)
                .WithMany(x => x.Stations)
                .HasForeignKey(x => x.ZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Tanks)
                .WithOne(x => x.Station)
                .HasForeignKey(x => x.StationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(x => x.Tanks)
                .HasField("_tanks")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(x => x.Area)
                .WithMany(x => x.Stations)
                .HasForeignKey(x => x.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.EnglishName)
                .HasColumnName(nameof(Station.EnglishName));

            builder.Property(x => x.ArabicName)
                .HasColumnName(nameof(Station.ArabicName));

            builder.Property(x => x.CityId)
                .HasColumnName(nameof(Station.CityId));

            builder.Property(x => x.ZoneId)
                .HasColumnName(nameof(Station.ZoneId));

            builder
            .HasIndex(x => new { x.AreaId, x.Id })
            .IsUnique(true);

            builder
            .HasIndex(x => new { x.CityId, x.Id })
            .IsUnique(true);

            
        }
    }
}


