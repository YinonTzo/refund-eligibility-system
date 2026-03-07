namespace RefundEligibilitySystem.Domain.Irepositories;

public interface IBudgetRepository
{
    Task<decimal> GetAvailableBudgetAsync();
}
