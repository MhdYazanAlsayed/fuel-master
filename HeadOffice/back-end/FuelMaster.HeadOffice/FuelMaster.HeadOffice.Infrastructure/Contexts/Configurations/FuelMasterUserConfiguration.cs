using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class FuelMasterUserConfiguration : IEntityTypeConfiguration<FuelMasterUser>
    {
        public void Configure(EntityTypeBuilder<FuelMasterUser> builder)
        {
            builder.HasOne(x => x.Employee)
                .WithOne(x => x.User)
                .HasForeignKey<FuelMasterUser>(x => x.EmployeeId)
                .IsRequired(false);
        }
    }
}


