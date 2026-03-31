using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;

namespace CustomerManagement.Application.Queries.Customers;

public class GetAllCustomersHandler
{
    private readonly ICustomerRepository _repository;

    public GetAllCustomersHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CustomerDto>> HandleAsync(GetAllCustomersQuery query, CancellationToken ct = default)
    {
        var customers = await _repository.GetAllAsync(ct);

        if (query.ActiveOnly)
            customers = customers.Where(c => c.IsActive);

        return customers.Select(c => new CustomerDto(
            c.Id, c.FirstName, c.LastName, c.FullName,
            c.Email, c.Phone, c.CompanyName,
            c.Segment, c.Classification,
            c.IsActive, c.CreatedAt, c.UpdatedAt));
    }
}