using Microsoft.EntityFrameworkCore;
using RefundEligibilitySystem.Domain.Entities;
using RefundEligibilitySystem.Domain.Irepositories;
using RefundEligibilitySystem.Infrastructure.Data;

namespace RefundEligibilitySystem.Infrastructure.Repositories;

public class CitizenRepository : ICitizenRepository
{
    private readonly RefundDbContext _db;

    public CitizenRepository(RefundDbContext db)
    {
        _db = db;
    }

    public async Task<Citizen?> GetByIdentityNumberAsync(string identityNumber)
    {
        return await _db.Citizens
            .FirstOrDefaultAsync(c => c.IdentityNumber == identityNumber);

    }
}
