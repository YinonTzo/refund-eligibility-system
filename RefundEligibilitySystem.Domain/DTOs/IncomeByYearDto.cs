namespace RefundEligibilitySystem.Domain.DTOs;

public class IncomeByYearDto
{
    public int Year { get; set; }
    public List<MonthlyIncomeDto> Months { get; set; }
}
