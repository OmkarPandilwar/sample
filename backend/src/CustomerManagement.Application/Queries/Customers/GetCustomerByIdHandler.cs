using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Application.Queries.Customers;

public class GetCustomerByIdHandler
{
    private readonly ICustomerRepository _repository;

    public GetCustomerByIdHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto> HandleAsync(GetCustomerByIdQuery query, CancellationToken ct = default)
    {
        var customer = await _repository.GetByIdWithDetailsAsync(query.Id, ct)
            ?? throw new CustomerNotFoundException(query.Id);

        return new CustomerDto(
            customer.Id, customer.FirstName, customer.LastName, customer.FullName,
            customer.Email, customer.Phone, customer.CompanyName,
            customer.Segment, customer.Classification,
            customer.IsActive, customer.CreatedAt, customer.UpdatedAt);
    }
}