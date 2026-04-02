using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Application.Interfaces;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Address>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
    Task<Address?> GetPrimaryAddressAsync(Guid customerId, CancellationToken ct = default);
    Task<bool> HasAnyAddressAsync(Guid customerId, CancellationToken ct = default);
    Task AddAsync(Address address, CancellationToken ct = default);
    void Update(Address address);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}