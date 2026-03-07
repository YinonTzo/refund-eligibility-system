namespace RefundEligibilitySystem.Domain.Entities;

public class Income
{
    public int IncomeId { get; set; }
    public int CitizenId { get; set; }
    public int IncomeYear { get; set; }
    public int IncomeMonth { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual Citizen Citizen { get; set; } = null!;
}
