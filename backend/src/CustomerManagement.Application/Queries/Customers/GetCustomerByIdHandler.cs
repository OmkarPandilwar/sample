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

    public async Task<CustomerDto> HandleAsync(
        GetCustomerByIdQuery query,
        CancellationToken ct = default)
    {
        var customer = await _repository.GetByIdWithDetailsAsync(query.Id, ct)
            ?? throw new CustomerNotFoundException(query.Id);

        if (!string.IsNullOrEmpty(query.AssignedRepId) && customer.AssignedSalesRepId != query.AssignedRepId)
            throw new UnauthorizedAccessException("You do not have access to this customer.");

        return new CustomerDto(
            customer.Id,
            customer.CustomerName,
            customer.Email,
            customer.Phone,
            customer.Website,
            customer.Industry,
            customer.CompanySize,
            customer.Classification,
            customer.Type,
            customer.Segment,
            customer.AccountValue,
            customer.AssignedSalesRepId,
            customer.IsActive,
            customer.CreatedDate,
            customer.ModifiedDate);
    }
}