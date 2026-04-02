using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.API.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize]
public class AddressesController : ControllerBase
{
    private readonly IAddressRepository _repository;

    public AddressesController(IAddressRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{customerId:guid}/addresses")]
    [Authorize(Policy = "AnyRole")]
    public async Task<IActionResult> GetByCustomer(
        Guid customerId, CancellationToken ct = default)
    {
        var addresses = await _repository
            .GetByCustomerIdAsync(customerId, ct);

        var result = addresses.Select(a => new AddressDto(
            a.Id, a.CustomerId, a.AddressType,
            a.Street, a.City, a.State,
            a.PostalCode, a.Country,
            a.IsPrimary, a.CreatedAt));

        return Ok(result);
    }

    [HttpPost("{customerId:guid}/addresses")]
    [Authorize(Policy = "AnyRole")]
    public async Task<IActionResult> Add(
        Guid customerId,
        [FromBody] CreateAddressRequest request,
        CancellationToken ct = default)
    {
        var address = Address.Create(
            customerId,
            request.AddressType,
            request.Street,
            request.City,
            request.State,
            request.PostalCode,
            request.Country,
            request.IsPrimary);

        await _repository.AddAsync(address, ct);
        await _repository.SaveChangesAsync(ct);

        return Created(
            $"/api/customers/{customerId}/addresses/{address.Id}",
            new AddressDto(
                address.Id, address.CustomerId,
                address.AddressType,
                address.Street, address.City,
                address.State, address.PostalCode,
                address.Country, address.IsPrimary,
                address.CreatedAt));
    }

    [HttpPut("{customerId:guid}/addresses/{addressId:guid}")]
    [Authorize(Policy = "AnyRole")]
    public async Task<IActionResult> Update(
        Guid customerId, Guid addressId,
        [FromBody] UpdateAddressRequest request,
        CancellationToken ct = default)
    {
        var address = await _repository.GetByIdAsync(addressId, ct);
        if (address == null || address.CustomerId != customerId)
            return NotFound();

        address.Update(
            request.AddressType,
            request.Street, request.City,
            request.State, request.PostalCode,
            request.Country);

        _repository.Update(address);
        await _repository.SaveChangesAsync(ct);

        return Ok(new AddressDto(
            address.Id, address.CustomerId,
            address.AddressType,
            address.Street, address.City,
            address.State, address.PostalCode,
            address.Country, address.IsPrimary,
            address.CreatedAt));
    }
}