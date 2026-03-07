using Microsoft.AspNetCore.Mvc;
using RefundEligibilitySystem.Domain.Exceptions;
using RefundEligibilitySystem.Domain.IServices;

namespace RefundEligibilitySystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitizensController : ControllerBase
{
    private readonly ICitizenService _citizenService;

    public CitizensController(ICitizenService citizenService)
    {
        _citizenService = citizenService;
    }

    [HttpGet("{identityNumber}/history")]
    public async Task<IActionResult> GetHistory(string identityNumber)
    {
        try
        {
            var result = await _citizenService.GetCitizenHistoryAsync(identityNumber);
            return Ok(result);
        }
        catch (BusinessRuleException ex) when (ex.ErrorCode == 40400)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
