using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RefundEligibilitySystem.Domain.Entities;
using RefundEligibilitySystem.Domain.Exceptions;
using RefundEligibilitySystem.Domain.Irepositories;
using RefundEligibilitySystem.Infrastructure.Data;

namespace RefundEligibilitySystem.Infrastructure.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly RefundDbContext _db;

    public ApplicationRepository(RefundDbContext db)
    {
        _db = db;
    }

    public async Task<List<Application>> GetPendingAsync()
    {
        return await _db.Applications
            .Where(a => a.Status == "Pending" || a.Status == "Calculated")
            .Include(a => a.Citizen)
            .OrderBy(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Application?> GetByIdAsync(int applicationId)
    {
        return await _db.Applications
            .Include(a => a.Citizen)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);
    }

    public async Task<List<Application>> GetByCitizenIdAsync(int citizenId, int? excludeApplicationId = null)
    {
        return await _db.Applications
            .Where(a => a.CitizenId == citizenId &&
                       (excludeApplicationId == null || a.ApplicationId != excludeApplicationId))
            .OrderByDescending(a => a.TaxYear)
            .ToListAsync();
    }

    public async Task<List<Income>> GetIncomesByCitizenIdAsync(int citizenId)
    {
        return await _db.Incomes
            .Where(i => i.CitizenId == citizenId)
            .OrderByDescending(i => i.IncomeYear)
            .ThenBy(i => i.IncomeMonth)
            .ToListAsync();
    }

    public async Task CalculateRefundAsync(int applicationId)
    {
        try
        {
            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_CalculateRefund @ApplicationId = {0}",
                applicationId);
        }
        catch (SqlException ex)
        {
            throw ex.Number switch
            {
                50000 => new BusinessRuleException(50000, "Application not found or not in Pending status."),
                50001 => new BusinessRuleException(50001, "Citizen already has an approved application for this tax year."),
                50002 => new BusinessRuleException(50002, "Fewer than 6 income months recorded for this tax year."),
                _ => new BusinessRuleException(ex.Number, ex.Message)
            };
        }
    }

    public async Task ApproveOrRejectAsync(int applicationId, bool isApproved)
    {
        try
        {
            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_ApproveApplication @ApplicationId = {0}, @IsApproved = {1}",
                applicationId,
                isApproved ? 1 : 0);
        }
        catch (SqlException ex)
        {
            throw ex.Number switch
            {
                50003 => new BusinessRuleException(50003, "Application not found or not in Calculated status."),
                50004 => new BusinessRuleException(50004, "Insufficient budget for this refund."),
                _ => new BusinessRuleException(ex.Number, ex.Message)
            };
        }
    }
}
