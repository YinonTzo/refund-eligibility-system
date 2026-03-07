using RefundEligibilitySystem.Domain.DTOs;
using RefundEligibilitySystem.Domain.Exceptions;
using RefundEligibilitySystem.Domain.Irepositories;
using RefundEligibilitySystem.Domain.IServices;

namespace RefundEligibilitySystem.Application.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IBudgetRepository _budgetRepository;

    public ApplicationService(
        IApplicationRepository applicationRepository,
        IBudgetRepository budgetRepository)
    {
        _applicationRepository = applicationRepository;
        _budgetRepository = budgetRepository;
    }

    public async Task<List<PendingApplicationDto>> GetPendingApplicationsAsync()
    {
        var applications = await _applicationRepository.GetPendingAsync();
        return applications.Select(a => new PendingApplicationDto
        {
            ApplicationId = a.ApplicationId,
            CitizenIdentityNumber = a.Citizen.IdentityNumber,
            CitizenFullName = a.Citizen.FullName,
            TaxYear = a.TaxYear,
            CreatedAt = a.CreatedAt
        }).ToList();
    }

    public async Task<CitizenApplicationsDto> GetApplicationDetailAsync(int applicationId)
    {
        var application = await _applicationRepository.GetByIdAsync(applicationId);

        if (application == null)
            throw new BusinessRuleException(50000, "Application not found.");

        var citizen = application.Citizen;

        var allIncomes = await _applicationRepository.GetIncomesByCitizenIdAsync(citizen.CitizenId);

        var incomesByYear = allIncomes
            .GroupBy(i => i.IncomeYear)
            .Select(g => new IncomeByYearDto
            {
                Year = g.Key,
                Months = g.Select(i => new MonthlyIncomeDto
                {
                    Month = i.IncomeMonth,
                    Amount = i.Amount
                }).OrderBy(m => m.Month).ToList()
            })
            .OrderByDescending(g => g.Year)
            .ToList();

        var pastApplications = await _applicationRepository
            .GetByCitizenIdAsync(citizen.CitizenId, excludeApplicationId: applicationId);

        var pastApplicationDtos = pastApplications.Select(a => new ApplicationDetailDto
        {
            ApplicationId = a.ApplicationId,
            TaxYear = a.TaxYear,
            Status = a.Status,
            CalculatedRefund = a.CalculatedRefund,
            AverageIncome = a.AverageIncome,
            CreatedAt = a.CreatedAt,
            OfficerDecisionDate = a.OfficerDecisionDate
        }).ToList();

        var currentApplicationDto = new ApplicationDetailDto
        {
            ApplicationId = application.ApplicationId,
            TaxYear = application.TaxYear,
            Status = application.Status,
            CalculatedRefund = application.CalculatedRefund,
            AverageIncome = application.AverageIncome,
            CreatedAt = application.CreatedAt,
            OfficerDecisionDate = application.OfficerDecisionDate
        };

        return new CitizenApplicationsDto
        {
            Citizen = new CitizenDto
            {
                CitizenId = citizen.CitizenId,
                IdentityNumber = citizen.IdentityNumber,
                FullName = citizen.FullName
            },
            Incomes = incomesByYear,
            PastApplications = pastApplicationDtos,
            CurrentApplication = currentApplicationDto
        };
    }

    public async Task<CalculateRefundResultDto> CalculateRefundAsync(int applicationId)
    {
        await _applicationRepository.CalculateRefundAsync(applicationId);

        var application = await _applicationRepository.GetByIdAsync(applicationId);
        var currentBudget = await _budgetRepository.GetAvailableBudgetAsync();

        return new CalculateRefundResultDto
        {
            CalculatedRefund = application!.CalculatedRefund,
            CurrentBudget = currentBudget
        };
    }

    public async Task ApproveOrRejectAsync(int applicationId, bool isApproved)
    {
        await _applicationRepository.ApproveOrRejectAsync(applicationId, isApproved);
    }
}
