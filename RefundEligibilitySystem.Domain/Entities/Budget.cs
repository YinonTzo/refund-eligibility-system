namespace RefundEligibilitySystem.Domain.Entities;

public class Budget
{
    public int BudgetId { get; set; }
    public decimal TotalAvailableBudget { get; set; }
    public DateTime UpdatedAt { get; set; }
}
