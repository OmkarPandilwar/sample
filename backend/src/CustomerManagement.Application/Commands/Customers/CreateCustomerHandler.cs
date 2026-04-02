using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;

namespace CustomerManagement.Application.Commands.Customers;

public class CreateCustomerHandler
{
    private readonly ICustomerRepository _repository;
    private readonly IDistributedCache _cache;
    private readonly IValidator<CreateCustomerCommand> _validator;

    public CreateCustomerHandler(
        ICustomerRepository repository,
        IDistributedCache cache,
        IValidator<CreateCustomerCommand> validator)
    {
        _repository = repository;
        _cache = cache;
        _validator = validator;
    }

    public async Task<CustomerDto> HandleAsync(
        CreateCustomerCommand command,
        CancellationToken ct = default)
    {
        await _validator.ValidateAndThrowAsync(command, ct);

        var emailExists = await _repository.EmailExistsAsync(command.Email, ct);
        if (emailExists)
            throw new DomainException($"A customer with email '{command.Email}' already exists.");

        // Requirement: Prevent duplicate customers (Email + CompanyName)
        var duplicate = await _repository.ExistsAsync(command.Email, command.CustomerName, ct);
        if (duplicate)
            throw new DomainException($"A customer with the same Email and Company Name already exists.");

        var customer = Customer.Create(
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

        await _repository.AddAsync(customer, ct);
        await _repository.SaveChangesAsync(ct);

        // Invalidate analytics cache
        await InvalidateCacheAsync(ct);

        return MapToDto(customer);
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

    public static CustomerDto MapToDto(Domain.Entities.Customer c) => new(
        c.Id, c.CustomerName, c.Email, c.Phone,
        c.Website, c.Industry, c.CompanySize,
        c.Classification, c.Type, c.Segment,
        c.AccountValue, c.AssignedSalesRepId,
        c.IsActive, c.CreatedDate, c.ModifiedDate);
}