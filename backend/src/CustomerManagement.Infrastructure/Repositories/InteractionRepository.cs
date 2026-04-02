using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Infrastructure.Repositories;

public class InteractionRepository : IInteractionRepository
{
    private readonly CustomerManagementDbContext _context;

    public InteractionRepository(CustomerManagementDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Interaction>> GetByCustomerIdAsync(
        Guid customerId, CancellationToken ct = default)
        => await _context.Interactions
            .AsNoTracking()
            .Where(i => i.CustomerId == customerId)
            .OrderByDescending(i => i.InteractionDate)
            .ToListAsync(ct);

    public async Task<Interaction?> GetByIdAsync(
        Guid id, CancellationToken ct = default)
        => await _context.Interactions
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, ct);

    public async Task<int> GetInteractionCountAsync(
        Guid customerId, DateTime since, CancellationToken ct = default)
        => await _context.Interactions
            .AsNoTracking()
            .CountAsync(i => i.CustomerId == customerId
                && i.InteractionDate >= since, ct);

    public async Task AddAsync(
        Interaction interaction, CancellationToken ct = default)
        => await _context.Interactions.AddAsync(interaction, ct);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}