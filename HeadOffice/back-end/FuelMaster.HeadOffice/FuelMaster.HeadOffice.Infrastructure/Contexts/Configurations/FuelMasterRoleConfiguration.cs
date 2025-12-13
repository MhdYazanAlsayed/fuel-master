using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class FuelMasterRoleConfiguration : IEntityTypeConfiguration<FuelMasterRole>
    {
        public void Configure(EntityTypeBuilder<FuelMasterRole> builder)
        {
            builder.HasMany(x => x.AreasOfAccess)
                .WithOne(x => x.Role)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(x => x.AreasOfAccess)
                .HasField("_areasOfAccess")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(x => x.ArabicName)
                .HasMaxLength(100)
                .HasColumnName(nameof(FuelMasterRole.ArabicName));

            builder.Property(x => x.EnglishName)
                .HasMaxLength(100)
                .HasColumnName(nameof(FuelMasterRole.EnglishName));
        }
    }
}

