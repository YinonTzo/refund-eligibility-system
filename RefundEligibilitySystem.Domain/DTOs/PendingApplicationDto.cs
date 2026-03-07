namespace RefundEligibilitySystem.Domain.DTOs;

public class PendingApplicationDto
{
    public int ApplicationId { get; set; }
    public string CitizenIdentityNumber { get; set; }
    public string CitizenFullName { get; set; }
    public int TaxYear { get; set; }
    public DateTime CreatedAt { get; set; }
}
