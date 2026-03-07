using RefundEligibilitySystem.Domain.Entities;

namespace RefundEligibilitySystem.Domain.Irepositories;

public interface ICitizenRepository
{
    Task<Citizen?> GetByIdentityNumberAsync(string identityNumber);
}
