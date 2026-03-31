using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Application.Commands.Contacts;

public class AddContactHandler
{
    private readonly IContactRepository _contactRepository;
    private readonly ICustomerRepository _customerRepository;

    public AddContactHandler(IContactRepository contactRepository, ICustomerRepository customerRepository)
    {
        _contactRepository = contactRepository;
        _customerRepository = customerRepository;
    }

    public async Task<ContactDto> HandleAsync(AddContactCommand command, CancellationToken ct = default)
    {
        var customerExists = await _customerRepository.GetByIdAsync(command.CustomerId, ct)
            ?? throw new CustomerNotFoundException(command.CustomerId);

        // Enforce one primary contact rule
        if (command.IsPrimary)
        {
            var hasPrimary = await _contactRepository.HasPrimaryContactAsync(command.CustomerId, ct);
            if (hasPrimary)
                throw new DomainException("This customer already has a primary contact.");
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

        return new ContactDto(
            contact.Id, contact.CustomerId,
            contact.FirstName, contact.LastName, contact.FullName,
            contact.Email, contact.Phone, contact.JobTitle,
            contact.IsPrimary, contact.IsActive, contact.CreatedAt);
    }
}