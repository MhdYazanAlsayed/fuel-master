using FuelMaster.HeadOffice.Core.Entities.Roles_Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class FuelMasterPermissionsConfiguration : IEntityTypeConfiguration<FuelMasterPermission>
    {
        public void Configure(EntityTypeBuilder<FuelMasterPermission> builder)
        {
            builder.HasOne(x => x.Role)
                .WithMany(x => x.AreasOfAccess)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AreaOfAccess)
                .WithMany()
                .HasForeignKey(x => x.AreaOfAccessId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

