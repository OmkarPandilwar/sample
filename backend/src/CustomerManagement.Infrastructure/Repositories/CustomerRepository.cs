using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerManagementDbContext _context;

    public CustomerRepository(CustomerManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<Customer?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default)
        => await _context.Customers
            .Include(c => c.Contacts)
            .Include(c => c.Addresses)
            .Include(c => c.Interactions)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken ct = default)
        => await _context.Customers
            .AsNoTracking()
            .OrderBy(c => c.LastName)
            .ToListAsync(ct);

    public async Task<IEnumerable<Customer>> GetBySegmentAsync(CustomerSegment segment, CancellationToken ct = default)
        => await _context.Customers
            .AsNoTracking()
            .Where(c => c.Segment == segment)
            .ToListAsync(ct);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        => await _context.Customers
            .AsNoTracking()
            .AnyAsync(c => c.Email == email.ToLowerInvariant(), ct);

    public async Task AddAsync(Customer customer, CancellationToken ct = default)
        => await _context.Customers.AddAsync(customer, ct);

    public void Update(Customer customer)
        => _context.Customers.Update(customer);

    public void Delete(Customer customer)
        => _context.Customers.Remove(customer);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}