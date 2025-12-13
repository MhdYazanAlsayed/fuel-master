using FuelMaster.HeadOffice.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FuelMaster.HeadOffice.Infrastructure.Contexts.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasOne(x => x.Nozzle)
                .WithMany()
                .HasForeignKey(x => x.NozzleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Volume)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.Totalizer)
                .HasColumnType("decimal(8,2)");

            builder.Property(x => x.TotalizerAfter)
                .HasColumnType("decimal(8,2)");
        }
    }
}


