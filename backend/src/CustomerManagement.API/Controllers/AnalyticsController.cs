using CustomerManagement.Application.Queries.Analytics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly GetCustomerAnalyticsHandler _handler;

    public AnalyticsController(GetCustomerAnalyticsHandler handler)
    {
        _handler = handler;
    }

    [HttpGet]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> Get(CancellationToken ct = default)
    {
        var result = await _handler.HandleAsync(new GetCustomerAnalyticsQuery(), ct);
        return Ok(result);
    }
}