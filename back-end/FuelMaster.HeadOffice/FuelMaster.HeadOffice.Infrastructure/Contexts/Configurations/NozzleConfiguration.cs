using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class NozzleConfiguration : IEntityTypeConfiguration<Nozzle>
    {
        public void Configure(EntityTypeBuilder<Nozzle> builder)
        {
        }
    }
}
