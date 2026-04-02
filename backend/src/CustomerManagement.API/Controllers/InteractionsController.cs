using CustomerManagement.Application.Commands.Interactions;
using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.API.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize]
public class InteractionsController : ControllerBase
{
    private readonly LogInteractionHandler _handler;
    private readonly IInteractionRepository _repository;

    public InteractionsController(
        LogInteractionHandler handler,
        IInteractionRepository repository)
    {
        _handler = handler;
        _repository = repository;
    }

    [HttpGet("{customerId:guid}/interactions")]
    [Authorize(Policy = "AnyRole")]
    public async Task<IActionResult> GetByCustomer(
        Guid customerId, CancellationToken ct = default)
    {
        var interactions = await _repository
            .GetByCustomerIdAsync(customerId, ct);

        var result = interactions.Select(i => new InteractionDto(
            i.Id, i.CustomerId, i.Type,
            i.Subject, i.Details,
            i.CreatedBy, i.InteractionDate, i.CreatedAt));

        return Ok(result);
    }

    [HttpPost("{customerId:guid}/interactions")]
    [Authorize(Policy = "AnyRole")]
    public async Task<IActionResult> Log(
        Guid customerId,
        [FromBody] CreateInteractionRequest request,
        CancellationToken ct = default)
    {
        var createdBy = User.Identity?.Name ?? "Unknown";

        var command = new LogInteractionCommand(
            customerId,
            request.Type,
            request.Subject,
            request.Details,
            createdBy,
            request.InteractionDate);

        var result = await _handler.HandleAsync(command, ct);
        return Created($"/api/customers/{customerId}/interactions/{result.Id}", result);
    }
}