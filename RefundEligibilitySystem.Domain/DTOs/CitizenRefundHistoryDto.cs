namespace RefundEligibilitySystem.Domain.DTOs;

public class CitizenRefundHistoryDto
{
    public CitizenDto Citizen { get; set; }
    public ApplicationDetailDto LatestApplication { get; set; }
    public List<ApplicationDetailDto> History { get; set; }
}
