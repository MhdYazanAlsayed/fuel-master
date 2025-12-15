using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class FuelMasterAreaOfAccessConfiguration : IEntityTypeConfiguration<FuelMasterAreaOfAccess>
    {
        public void Configure(EntityTypeBuilder<FuelMasterAreaOfAccess> builder)
        {
            builder.HasMany<FuelMasterPermission>()
                .WithOne(x => x.AreaOfAccess)
                .HasForeignKey(x => x.AreaOfAccessId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.AreaOfAccess)
                .HasColumnName(nameof(FuelMasterAreaOfAccess.AreaOfAccess))
                .HasConversion<int>();

            builder.Property(x => x.EnglishName)
                .HasMaxLength(100)
                .HasColumnName(nameof(FuelMasterAreaOfAccess.EnglishName));

            builder.Property(x => x.ArabicName)
                .HasMaxLength(100)
                .HasColumnName(nameof(FuelMasterAreaOfAccess.ArabicName));

            builder.Property(x => x.EnglishDescription)
                .HasMaxLength(500)
                .HasColumnName(nameof(FuelMasterAreaOfAccess.EnglishDescription));

            builder.Property(x => x.ArabicDescription)
                .HasMaxLength(500)
                .HasColumnName(nameof(FuelMasterAreaOfAccess.ArabicDescription));
        }
    }
}



