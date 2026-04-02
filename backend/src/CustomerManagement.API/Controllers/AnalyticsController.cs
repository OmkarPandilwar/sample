using CustomerManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.API.Controllers;

[ApiController]
[Route("api/customers/analytics")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> Get(CancellationToken ct = default)
    {
        var result = await _analyticsService.GetAnalyticsAsync(ct);
        return Ok(result);
    }

    [HttpGet("lifetime-value")]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> GetLifetimeValue(CancellationToken ct = default)
    {
        var result = await _analyticsService.GetLifetimeValueAsync(ct);
        return Ok(result);
    }

    [HttpGet("health-score")]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> GetHealthScore(CancellationToken ct = default)
    {
        var result = await _analyticsService.GetHealthScoresAsync(ct);
        return Ok(result);
    }

    [HttpGet("segmentation-distribution")]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> GetSegmentation(CancellationToken ct = default)
    {
        var result = await _analyticsService.GetSegmentationAsync(ct);
        return Ok(result);
    }

    [HttpGet("churn-risk")]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> GetChurnRisk(CancellationToken ct = default)
    {
        var result = await _analyticsService.GetChurnRiskAsync(ct);
        return Ok(result);
    }
}