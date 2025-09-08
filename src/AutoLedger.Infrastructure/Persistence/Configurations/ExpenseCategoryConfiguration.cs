using AutoLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AutoLedger.Infrastructure.Persistence.Configurations;

public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
{
    public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasMany(c => c.Expenses)
               .WithOne(e => e.Category)
               .HasForeignKey(e => e.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}