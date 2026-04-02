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

    public async Task<IEnumerable<CustomerDto>> HandleAsync(
        GetAllCustomersQuery query,
        CancellationToken ct = default)
    {
        var customers = await _repository.GetAllAsync(query.AssignedRepId, ct);

        if (query.ActiveOnly)
            customers = customers.Where(c => c.IsActive);

        // Simple pagination
        customers = customers
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize);

        return customers.Select(c => new CustomerDto(
            c.Id,
            c.CustomerName,
            c.Email,
            c.Phone,
            c.Website,
            c.Industry,
            c.CompanySize,
            c.Classification,
            c.Type,
            c.Segment,
            c.AccountValue,
            c.AssignedSalesRepId,
            c.IsActive,
            c.CreatedDate,
            c.ModifiedDate));
    }
}