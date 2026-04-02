using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Exceptions;
using Microsoft.Extensions.Caching.Distributed;

namespace CustomerManagement.Application.Commands.Customers;

public class DeleteCustomerHandler
{
    private readonly ICustomerRepository _repository;
    private readonly IDistributedCache _cache;

    public DeleteCustomerHandler(
        ICustomerRepository repository,
        IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task HandleAsync(DeleteCustomerCommand command, CancellationToken ct = default)
    {
        var customer = await _repository.GetByIdWithDetailsAsync(command.Id, ct)
            ?? throw new CustomerNotFoundException(command.Id);

        // Security/Restriction check
        if (customer.Classification == CustomerManagement.Domain.Enums.CustomerClassification.VIP)
            throw new DomainException("VIP customers cannot be deleted.");

        customer.Deactivate();
        _repository.Update(customer);
        await _repository.SaveChangesAsync(ct);

        // Invalidate analytics cache
        await InvalidateCacheAsync(ct);
    }

    private async Task InvalidateCacheAsync(CancellationToken ct)
    {
        var keys = new[] { "analytics_overview", "analytics_lifetime", "analytics_health", "analytics_segmentation", "analytics_churn" };
        foreach (var key in keys)
            await _cache.RemoveAsync(key, ct);
    }
}