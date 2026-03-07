using RefundEligibilitySystem.Domain.DTOs;

namespace RefundEligibilitySystem.Domain.IServices;

public interface IApplicationService
{
    Task<List<PendingApplicationDto>> GetPendingApplicationsAsync();
    Task<CitizenApplicationsDto> GetApplicationDetailAsync(int applicationId);
    Task<CalculateRefundResultDto> CalculateRefundAsync(int applicationId);
    Task ApproveOrRejectAsync(int applicationId, bool isApproved);
}
