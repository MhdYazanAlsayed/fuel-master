using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class FuelMasterGroupConfiguration : IEntityTypeConfiguration<FuelMasterGroup>
    {
        public void Configure(EntityTypeBuilder<FuelMasterGroup> builder)
        {
            builder.HasMany(x => x.Permissions)
                .WithOne(x => x.FuelMasterGroup)
                .HasForeignKey(x => x.FuelMasterGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(x => x.Permissions)
                .HasField("_permissions")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.ArabicName)
                .HasColumnName(nameof(FuelMasterGroup.ArabicName));

            builder.Property(x => x.EnglishName)
                .HasColumnName(nameof(FuelMasterGroup.EnglishName));
        }
    }
}


