namespace RefundEligibilitySystem.Domain.DTOs;

public class CalculateRefundResultDto
{
    public decimal CalculatedRefund { get; set; }
    public decimal CurrentBudget { get; set; }
}
