using CustomerManagement.Domain.Entities;

namespace CustomerManagement.Application.Interfaces;

public interface IInteractionRepository
{
    Task<IEnumerable<Interaction>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
    Task AddAsync(Interaction interaction, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}