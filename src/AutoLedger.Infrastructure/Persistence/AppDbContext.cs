using AutoLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoLedger.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserConfirme> Confirmers { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
    public DbSet<Odometer> Odometers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
