using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Application.Interfaces;

public interface IContactRepository
{
    Task<Contact?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Contact>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
    Task<bool> HasPrimaryContactAsync(Guid customerId, CancellationToken ct = default);
    Task AddAsync(Contact contact, CancellationToken ct = default);
    void Update(Contact contact);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}