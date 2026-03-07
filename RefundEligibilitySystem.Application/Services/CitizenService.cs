using RefundEligibilitySystem.Domain.DTOs;
using RefundEligibilitySystem.Domain.Exceptions;
using RefundEligibilitySystem.Domain.Irepositories;
using RefundEligibilitySystem.Domain.IServices;

namespace RefundEligibilitySystem.Application.Services;

public class CitizenService : ICitizenService
{
    private readonly ICitizenRepository _citizenRepository;
    private readonly IApplicationRepository _applicationRepository;

    public CitizenService(ICitizenRepository citizenRepository, IApplicationRepository applicationRepository)
    {
        _citizenRepository = citizenRepository;
        _applicationRepository = applicationRepository;
    }

    public async Task<CitizenRefundHistoryDto> GetCitizenHistoryAsync(string identityNumber)
    {
        var citizen = await _citizenRepository.GetByIdentityNumberAsync(identityNumber);

        if (citizen == null)
            throw new BusinessRuleException(40400, "Citizen not found.");

        var applications = await _applicationRepository.GetByCitizenIdAsync(citizen.CitizenId);

        var applicationDtos = applications.Select(a => new ApplicationDetailDto
        {
            ApplicationId = a.ApplicationId,
            TaxYear = a.TaxYear,
            Status = a.Status,
            CalculatedRefund = a.CalculatedRefund,
            AverageIncome = a.AverageIncome,
            CreatedAt = a.CreatedAt,
            OfficerDecisionDate = a.OfficerDecisionDate
        }).ToList();

        var latest = applicationDtos.FirstOrDefault();

        var pastApplications = applicationDtos.Skip(1).ToList();

        return new CitizenRefundHistoryDto
        {
            Citizen = new CitizenDto
            {
                CitizenId = citizen.CitizenId,
                IdentityNumber = citizen.IdentityNumber,
                FullName = citizen.FullName
            },
            LatestApplication = latest,
            History = pastApplications
        };
    }
}
