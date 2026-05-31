using BusinessOps.Api.Dtos;
using BusinessOps.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BusinessOps.Api.Controllers;

[ApiController]
[Route("api/insights")]
public class InsightsController : ControllerBase
{
    private readonly OperationsInsightsService _insightsService;

    public InsightsController(OperationsInsightsService insightsService)
    {
        _insightsService = insightsService;
    }

    [HttpGet("operations-summary")]
    public async Task<ActionResult<OperationsSummaryDto>> GetOperationsSummary()
    {
        var summary = await _insightsService.GetOperationsSummaryAsync();

        return Ok(summary);
    }
}