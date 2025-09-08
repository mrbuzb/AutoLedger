using AutoLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AutoLedger.Infrastructure.Persistence.Configurations;

public class OdometerConfiguration : IEntityTypeConfiguration<Odometer>
{
    public void Configure(EntityTypeBuilder<Odometer> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Value)
               .IsRequired();

        builder.Property(o => o.Date)
               .IsRequired();

        builder.HasOne(o => o.Vehicle)
               .WithMany(v => v.Odometers)
               .HasForeignKey(o => o.VehicleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}