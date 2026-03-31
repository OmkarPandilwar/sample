using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Domain.Entities;

public class Contact
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public string? JobTitle { get; private set; }
    public bool IsPrimary { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation
    public Customer? Customer { get; private set; }

    private Contact() { }

    public static Contact Create(
        Guid customerId,
        string firstName,
        string lastName,
        string email,
        bool isPrimary,
        string? phone = null,
        string? jobTitle = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("Contact first name is required.");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Contact email is required.");

        return new Contact
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = email.ToLowerInvariant(),
            Phone = phone,
            JobTitle = jobTitle,
            IsPrimary = isPrimary,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MakePrimary() => IsPrimary = true;
    public void RemovePrimary() => IsPrimary = false;
    public void Deactivate() => IsActive = false;

    public string FullName => $"{FirstName} {LastName}";
}