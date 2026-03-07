using Microsoft.AspNetCore.Mvc;
using RefundEligibilitySystem.Domain.DTOs;
using RefundEligibilitySystem.Domain.Exceptions;
using RefundEligibilitySystem.Domain.IServices;

namespace RefundEligibilitySystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;

    public ApplicationsController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _applicationService.GetPendingApplicationsAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        try
        {
            var result = await _applicationService.GetApplicationDetailAsync(id);
            return Ok(result);
        }
        catch (BusinessRuleException ex) when (ex.ErrorCode == 50000)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/calculate")]
    public async Task<IActionResult> Calculate(int id)
    {
        try
        {
            var result = await _applicationService.CalculateRefundAsync(id);
            return Ok(result);
        }
        catch (BusinessRuleException ex)
        {
            return ex.ErrorCode switch
            {
                50000 => NotFound(new { message = ex.Message }),
                50001 => Conflict(new { message = ex.Message }),
                50002 => UnprocessableEntity(new { message = ex.Message }),
                _ => BadRequest(new { message = ex.Message })
            };
        }
    }

    [HttpPost("{id}/decide")]
    public async Task<IActionResult> Decide(int id, [FromBody] OfficerDecisionRequest request)
    {
        try
        {
            await _applicationService.ApproveOrRejectAsync(id, request.IsApproved);
            return Ok(new { message = request.IsApproved ? "Application approved." : "Application rejected." });
        }
        catch (BusinessRuleException ex)
        {
            return ex.ErrorCode switch
            {
                50003 => NotFound(new { message = ex.Message }),
                50004 => Conflict(new { message = ex.Message }),
                _ => BadRequest(new { message = ex.Message })
            };
        }
    }
}
