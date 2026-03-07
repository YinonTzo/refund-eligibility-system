using Microsoft.EntityFrameworkCore;
using RefundEligibilitySystem.Domain.Irepositories;
using RefundEligibilitySystem.Infrastructure.Data;

namespace RefundEligibilitySystem.Infrastructure.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly RefundDbContext _db;

    public BudgetRepository(RefundDbContext db)
    {
        _db = db;
    }

    public async Task<decimal> GetAvailableBudgetAsync()
    {
        var budget = await _db.Budget.FirstOrDefaultAsync();
        return budget.TotalAvailableBudget;
    }
}
