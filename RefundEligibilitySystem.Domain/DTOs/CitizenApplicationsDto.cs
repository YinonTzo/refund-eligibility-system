namespace RefundEligibilitySystem.Domain.DTOs;

public class CitizenApplicationsDto
{
    public CitizenDto Citizen { get; set; }
    public List<IncomeByYearDto> Incomes { get; set; }
    public List<ApplicationDetailDto> PastApplications { get; set; }
    public ApplicationDetailDto CurrentApplication { get; set; }
}
