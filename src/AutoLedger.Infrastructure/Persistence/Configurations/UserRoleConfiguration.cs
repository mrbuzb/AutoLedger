using AutoLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoLedger.Infrastructure.Persistence.Configurations;
public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name).HasMaxLength(50).IsRequired();
        builder.Property(r => r.Description).HasMaxLength(255);

        builder.HasData(
            new UserRole { Id = 1, Name = "SuperAdmin",Description = "." },
            new UserRole { Id = 2, Name = "Admin",Description = "." },
            new UserRole { Id = 3, Name = "User" , Description = "." }
        );
    }
}
