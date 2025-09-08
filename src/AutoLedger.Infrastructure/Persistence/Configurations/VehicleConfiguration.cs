using AutoLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AutoLedger.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Make)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(v => v.Model)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(v => v.Plate)
               .HasMaxLength(20);

        builder.HasMany(v => v.Expenses)
               .WithOne(e => e.Vehicle)
               .HasForeignKey(e => e.VehicleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(v => v.User)
               .WithMany(u => u.Vehicles)
               .HasForeignKey(v => v.UserId);
    }
}