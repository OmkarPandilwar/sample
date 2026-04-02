using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;

namespace CustomerManagement.Application.Commands.Customers;

public class UpdateCustomerHandler
{
    private readonly ICustomerRepository _repository;
    private readonly IDistributedCache _cache;
    private readonly IValidator<UpdateCustomerCommand> _validator;

    public UpdateCustomerHandler(
        ICustomerRepository repository,
        IDistributedCache cache,
        IValidator<UpdateCustomerCommand> _validator)
    {
        _repository = repository;
        _cache = cache;
        this._validator = _validator;
    }

    public async Task<CustomerDto> HandleAsync(
        UpdateCustomerCommand command,
        CancellationToken ct = default)
    {
        await _validator.ValidateAndThrowAsync(command, ct);

        var customer = await _repository.GetByIdAsync(command.Id, ct)
            ?? throw new CustomerNotFoundException(command.Id);

        customer.Update(
            command.CustomerName,
            command.Email,
            command.Classification,
            command.Type,
            command.Segment,
            command.Phone,
            command.Website,
            command.Industry,
            command.CompanySize,
            command.AccountValue,
            command.AssignedSalesRepId);

        _repository.Update(customer);
        await _repository.SaveChangesAsync(ct);

        // Invalidate analytics cache
        await InvalidateCacheAsync(ct);

        return CreateCustomerHandler.MapToDto(customer);
    }

    private async Task InvalidateCacheAsync(CancellationToken ct)
    {
        var keys = new[] { 
            "analytics_overview", 
            "analytics_lifetime", 
            "analytics_health", 
            "analytics_segmentation", 
            "analytics_churn" 
        };
        foreach (var key in keys)
            await _cache.RemoveAsync(key, ct);
    }
}