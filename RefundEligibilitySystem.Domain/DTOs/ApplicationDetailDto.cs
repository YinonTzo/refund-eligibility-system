namespace RefundEligibilitySystem.Domain.DTOs;

public class ApplicationDetailDto
{
    public int ApplicationId { get; set; }
    public int TaxYear { get; set; }
    public string Status { get; set; }
    public decimal CalculatedRefund { get; set; }
    public decimal AverageIncome { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? OfficerDecisionDate { get; set; }
}