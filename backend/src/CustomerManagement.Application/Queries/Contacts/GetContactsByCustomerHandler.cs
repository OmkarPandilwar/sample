using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;

namespace CustomerManagement.Application.Queries.Contacts;

public class GetContactsByCustomerHandler
{
    private readonly IContactRepository _repository;

    public GetContactsByCustomerHandler(IContactRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ContactDto>> HandleAsync(GetContactsByCustomerQuery query, CancellationToken ct = default)
    {
        var contacts = await _repository.GetByCustomerIdAsync(query.CustomerId, ct);

        return contacts.Select(c => new ContactDto(
            c.Id, c.CustomerId,
            c.FirstName, c.LastName, c.FullName,
            c.Email, c.Phone, c.JobTitle,
            c.IsPrimary, c.IsActive, c.CreatedAt));
    }
}