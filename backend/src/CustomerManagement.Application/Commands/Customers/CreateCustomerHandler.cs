using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Application.Commands.Customers;

public class CreateCustomerHandler
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto> HandleAsync(CreateCustomerCommand command, CancellationToken ct = default)
    {
        var emailExists = await _repository.EmailExistsAsync(command.Email, ct);
        if (emailExists)
            throw new DomainException($"A customer with email '{command.Email}' already exists.");

        var customer = Customer.Create(
            command.FirstName,
            command.LastName,
            command.Email,
            command.Segment,
            command.Classification,
            command.Phone,
            command.CompanyName);

        await _repository.AddAsync(customer, ct);
        await _repository.SaveChangesAsync(ct);

        return MapToDto(customer);
    }

    private static CustomerDto MapToDto(Customer c) => new(
        c.Id, c.FirstName, c.LastName, c.FullName,
        c.Email, c.Phone, c.CompanyName,
        c.Segment, c.Classification,
        c.IsActive, c.CreatedAt, c.UpdatedAt);
}