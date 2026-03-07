using RefundEligibilitySystem.Domain.Entities;

namespace RefundEligibilitySystem.Domain.Irepositories;

public interface IApplicationRepository
{
    Task<List<Application>> GetPendingAsync();
    Task<Application?> GetByIdAsync(int applicationId);
    Task CalculateRefundAsync(int applicationId);
    Task<List<Application>> GetByCitizenIdAsync(int citizenId, int? excludeApplicationId = null);
    Task<List<Income>> GetIncomesByCitizenIdAsync(int citizenId);
    Task ApproveOrRejectAsync(int applicationId, bool isApproved);
}
