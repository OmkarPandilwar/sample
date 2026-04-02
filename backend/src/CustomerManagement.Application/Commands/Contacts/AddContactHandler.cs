using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;

namespace CustomerManagement.Application.Commands.Contacts;

public class AddContactHandler
{
    private readonly IContactRepository _contactRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IDistributedCache _cache;
    private readonly IValidator<AddContactCommand> _validator;

    public AddContactHandler(
        IContactRepository contactRepository, 
        ICustomerRepository customerRepository,
        IDistributedCache cache,
        IValidator<AddContactCommand> validator)
    {
        _contactRepository = contactRepository;
        _customerRepository = customerRepository;
        _cache = cache;
        _validator = validator;
    }

    public async Task<ContactDto> HandleAsync(AddContactCommand command, CancellationToken ct = default)
    {
        await _validator.ValidateAndThrowAsync(command, ct);

        var customerExists = await _customerRepository.GetByIdAsync(command.CustomerId, ct)
            ?? throw new CustomerNotFoundException(command.CustomerId);

        // Enforce one primary contact rule
        if (command.IsPrimary)
        {
            var existingPrimary = await _contactRepository.GetPrimaryContactAsync(command.CustomerId, ct);
            if (existingPrimary != null)
            {
                existingPrimary.RemovePrimary();
                _contactRepository.Update(existingPrimary);
            }
        }

        var contact = Contact.Create(
            command.CustomerId,
            command.FirstName,
            command.LastName,
            command.Email,
            command.IsPrimary,
            command.Phone,
            command.JobTitle);

        await _contactRepository.AddAsync(contact, ct);
        await _contactRepository.SaveChangesAsync(ct);

        // Invalidate analytics cache
        await InvalidateCacheAsync(ct);

        return new ContactDto(
            contact.Id, contact.CustomerId,
            contact.FirstName, contact.LastName, contact.FullName,
            contact.Email, contact.Phone, contact.JobTitle,
            contact.IsPrimary, contact.IsActive, contact.CreatedAt);
    }

    private async Task InvalidateCacheAsync(CancellationToken ct)
    {
        var keys = new[] { "analytics_overview", "analytics_lifetime", "analytics_health", "analytics_segmentation", "analytics_churn" };
        foreach (var key in keys)
            await _cache.RemoveAsync(key, ct);
    }
}