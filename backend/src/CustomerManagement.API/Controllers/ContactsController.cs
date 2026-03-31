using CustomerManagement.Application.Commands.Contacts;
using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Queries.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContactsController : ControllerBase
{
    private readonly AddContactHandler _addHandler;
    private readonly GetContactsByCustomerHandler _getHandler;

    public ContactsController(AddContactHandler addHandler, GetContactsByCustomerHandler getHandler)
    {
        _addHandler = addHandler;
        _getHandler = getHandler;
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> GetByCustomer(Guid customerId, CancellationToken ct = default)
    {
        var result = await _getHandler.HandleAsync(new GetContactsByCustomerQuery(customerId), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "ManagerOrAbove")]
    public async Task<IActionResult> Add([FromBody] CreateContactRequest request, CancellationToken ct = default)
    {
        var command = new AddContactCommand(
            request.CustomerId, request.FirstName, request.LastName,
            request.Email, request.Phone, request.JobTitle, request.IsPrimary);

        var result = await _addHandler.HandleAsync(command, ct);
        return Created($"/api/contacts/{result.Id}", result);
    }
}