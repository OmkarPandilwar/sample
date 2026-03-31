using CustomerManagement.Application.Commands.Interactions;
using CustomerManagement.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InteractionsController : ControllerBase
{
    private readonly LogInteractionHandler _handler;

    public InteractionsController(LogInteractionHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    [Authorize(Policy = "AnyRole")]
    public async Task<IActionResult> Log([FromBody] CreateInteractionRequest request, CancellationToken ct = default)
    {
        var createdBy = User.Identity?.Name ?? "Unknown";

        var command = new LogInteractionCommand(
            request.CustomerId, request.Type,
            request.Notes, createdBy, request.InteractionDate);

        var result = await _handler.HandleAsync(command, ct);
        return Created($"/api/interactions/{result.Id}", result);
    }
}