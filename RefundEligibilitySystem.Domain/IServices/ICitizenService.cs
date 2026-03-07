using RefundEligibilitySystem.Domain.DTOs;

namespace RefundEligibilitySystem.Domain.IServices;

public interface ICitizenService
{
    Task<CitizenRefundHistoryDto> GetCitizenHistoryAsync(string identityNumber);
}
