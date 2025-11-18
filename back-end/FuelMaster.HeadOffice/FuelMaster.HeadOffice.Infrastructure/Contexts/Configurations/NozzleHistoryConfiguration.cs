using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class NozzleHistoryConfiguration : IEntityTypeConfiguration<NozzleHistory>
    {
        public void Configure(EntityTypeBuilder<NozzleHistory> builder)
        {
            builder.HasOne(x => x.Nozzle)
                .WithMany()
                .HasForeignKey(x => x.NozzleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Station)
                .WithMany()
                .HasForeignKey(x => x.StationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Tank)
                .WithMany()
                .HasForeignKey(x => x.TankId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Volume)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}


