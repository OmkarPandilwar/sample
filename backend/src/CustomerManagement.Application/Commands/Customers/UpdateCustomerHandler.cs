using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Application.Commands.Customers;

public class UpdateCustomerHandler
{
    private readonly ICustomerRepository _repository;

    public UpdateCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto> HandleAsync(UpdateCustomerCommand command, CancellationToken ct = default)
    {
        var customer = await _repository.GetByIdAsync(command.Id, ct)
            ?? throw new CustomerNotFoundException(command.Id);

        customer.Update(
            command.FirstName,
            command.LastName,
            command.Email,
            command.Segment,
            command.Classification,
            command.Phone,
            command.CompanyName);

        _repository.Update(customer);
        await _repository.SaveChangesAsync(ct);

        return new CustomerDto(
            customer.Id, customer.FirstName, customer.LastName, customer.FullName,
            customer.Email, customer.Phone, customer.CompanyName,
            customer.Segment, customer.Classification,
            customer.IsActive, customer.CreatedAt, customer.UpdatedAt);
    }
}