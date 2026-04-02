using CustomerManagement.Application.Commands.Contacts;
using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.API.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize]
public class ContactsController : ControllerBase
{
    private readonly AddContactHandler _addHandler;
    private readonly IContactRepository _contactRepository;

    public ContactsController(
        AddContactHandler addHandler,
        IContactRepository contactRepository)
    {
        _addHandler = addHandler;
        _contactRepository = contactRepository;
    }

    [HttpGet("{customerId:guid}/contacts")]
    [Authorize(Policy = "AnyRole")]
    public async Task<IActionResult> GetByCustomer(
        Guid customerId, CancellationToken ct = default)
    {
        var contacts = await _contactRepository
            .GetByCustomerIdAsync(customerId, ct);

        var result = contacts.Select(c => new ContactDto(
            c.Id, c.CustomerId,
            c.FirstName, c.LastName, c.FullName,
            c.Email, c.Phone, c.JobTitle,
            c.IsPrimary, c.IsActive, c.CreatedAt));

        return Ok(result);
    }

    [HttpPost("{customerId:guid}/contacts")]
    [Authorize(Policy = "AnyRole")]
    public async Task<IActionResult> Add(
        Guid customerId,
        [FromBody] CreateContactRequest request,
        CancellationToken ct = default)
    {
        var command = new AddContactCommand(
            customerId,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.JobTitle,
            request.IsPrimary);

        var result = await _addHandler.HandleAsync(command, ct);
        return Created(
            $"/api/customers/{customerId}/contacts/{result.Id}",
            result);
    }
}