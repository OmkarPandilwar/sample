using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Application.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Customer?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Customer>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<Customer>> GetBySegmentAsync(CustomerSegment segment, CancellationToken ct = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
    Task AddAsync(Customer customer, CancellationToken ct = default);
    void Update(Customer customer);
    void Delete(Customer customer);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}