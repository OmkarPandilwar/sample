using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Infrastructure.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly CustomerManagementDbContext _context;

    public ContactRepository(CustomerManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Contact?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IEnumerable<Contact>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default)
        => await _context.Contacts
            .AsNoTracking()
            .Where(c => c.CustomerId == customerId)
            .OrderByDescending(c => c.IsPrimary)
            .ToListAsync(ct);

    public async Task<bool> HasPrimaryContactAsync(Guid customerId, CancellationToken ct = default)
        => await _context.Contacts
            .AsNoTracking()
            .AnyAsync(c => c.CustomerId == customerId && c.IsPrimary && c.IsActive, ct);

    public async Task AddAsync(Contact contact, CancellationToken ct = default)
        => await _context.Contacts.AddAsync(contact, ct);

    public void Update(Contact contact)
        => _context.Contacts.Update(contact);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}