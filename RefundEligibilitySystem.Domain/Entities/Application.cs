namespace RefundEligibilitySystem.Domain.Entities;

public class Application
{
    public int ApplicationId { get; set; }
    public int CitizenId { get; set; }
    public int TaxYear { get; set; }
    public decimal CalculatedRefund { get; set; }
    public decimal AverageIncome { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime? OfficerDecisionDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual Citizen Citizen { get; set; } = null!;
}
