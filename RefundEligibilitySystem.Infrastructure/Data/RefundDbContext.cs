namespace RefundEligibilitySystem.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using RefundEligibilitySystem.Domain.Entities;

public class RefundDbContext(DbContextOptions<RefundDbContext> options) : DbContext(options)
{
    public DbSet<Citizen> Citizens => Set<Citizen>();
    public DbSet<Income> Incomes => Set<Income>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<Budget> Budget => Set<Budget>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Citizen>(entity =>
        {
            entity.HasIndex(e => e.IdentityNumber).IsUnique();
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasIndex(e => new { e.CitizenId, e.IncomeYear, e.IncomeMonth }).IsUnique();

            entity.Property(e => e.Amount).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasIndex(e => new { e.CitizenId, e.TaxYear }).IsUnique();
            entity.Property(e => e.CalculatedRefund).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.Property(e => e.TotalAvailableBudget).HasPrecision(18, 2);
        });
    }
}