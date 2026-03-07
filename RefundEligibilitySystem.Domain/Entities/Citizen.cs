namespace RefundEligibilitySystem.Domain.Entities;

public class Citizen
{
    public int CitizenId { get; set; }
    public string IdentityNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}