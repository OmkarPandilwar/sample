using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly CustomerManagementDbContext _context;

    public AddressRepository(CustomerManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Address?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IEnumerable<Address>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
        => await _context.Addresses
            .AsNoTracking()
            .Where(a => a.CustomerId == customerId)
            .OrderByDescending(a => a.IsPrimary)
            .ToListAsync(ct);

    public async Task<Address?> GetPrimaryAddressAsync(Guid customerId, CancellationToken ct = default)
        => await _context.Addresses
            .FirstOrDefaultAsync(a => a.CustomerId == customerId && a.IsPrimary, ct);

    public async Task<bool> HasAnyAddressAsync(Guid customerId, CancellationToken ct = default)
        => await _context.Addresses.AnyAsync(a => a.CustomerId == customerId, ct);

    public async Task AddAsync(Address address, CancellationToken ct = default)
        => await _context.Addresses.AddAsync(address, ct);

    public void Update(Address address)
        => _context.Addresses.Update(address);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}