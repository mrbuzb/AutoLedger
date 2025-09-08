using AutoLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AutoLedger.Infrastructure.Persistence.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Amount)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(e => e.Note)
               .HasMaxLength(200);

        builder.Property(e => e.Date)
               .IsRequired();

        builder.HasOne(e => e.Vehicle)
               .WithMany(v => v.Expenses)
               .HasForeignKey(e => e.VehicleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}