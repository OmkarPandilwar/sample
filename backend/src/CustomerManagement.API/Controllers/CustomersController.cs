using CustomerManagement.Application.Commands.Customers;
using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Queries.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly GetAllCustomersHandler _getAllHandler;
    private readonly GetCustomerByIdHandler _getByIdHandler;
    private readonly CreateCustomerHandler _createHandler;
    private readonly UpdateCustomerHandler _updateHandler;
    private readonly DeleteCustomerHandler _deleteHandler;

    public CustomersController(
        GetAllCustomersHandler getAllHandler,
        GetCustomerByIdHandler getByIdHandler,
        CreateCustomerHandler createHandler,
        UpdateCustomerHandler updateHandler,
        DeleteCustomerHandler deleteHandler)
    {
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
    }

    [HttpGet]
    [Authorize(Policy = "AnyRole")]
    public async Task<IActionResult> GetAll([FromQuery] bool activeOnly = false, CancellationToken ct = default)
    {
        var result = await _getAllHandler.HandleAsync(new GetAllCustomersQuery(activeOnly), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "AnyRole")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await _getByIdHandler.HandleAsync(new GetCustomerByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request, CancellationToken ct = default)
    {
        var command = new CreateCustomerCommand(
            request.FirstName, request.LastName, request.Email,
            request.Phone, request.CompanyName,
            request.Segment, request.Classification);

        var result = await _createHandler.HandleAsync(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request, CancellationToken ct = default)
    {
        var command = new UpdateCustomerCommand(
            id, request.FirstName, request.LastName, request.Email,
            request.Phone, request.CompanyName,
            request.Segment, request.Classification);

        var result = await _updateHandler.HandleAsync(command, ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        await _deleteHandler.HandleAsync(new DeleteCustomerCommand(id), ct);
        return NoContent();
    }
}