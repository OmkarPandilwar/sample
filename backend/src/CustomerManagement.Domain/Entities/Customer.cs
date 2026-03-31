using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public string? CompanyName { get; private set; }
    public CustomerSegment Segment { get; private set; }
    public CustomerClassification Classification { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation properties
    private readonly List<Contact> _contacts = new();
    private readonly List<Address> _addresses = new();
    private readonly List<Interaction> _interactions = new();

    public IReadOnlyCollection<Contact> Contacts => _contacts.AsReadOnly();
    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();
    public IReadOnlyCollection<Interaction> Interactions => _interactions.AsReadOnly();

    // Required by EF Core
    private Customer() { }

    public static Customer Create(
        string firstName,
        string lastName,
        string email,
        CustomerSegment segment,
        CustomerClassification classification,
        string? phone = null,
        string? companyName = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required.");

        // Validate email via value object
        var validatedEmail = new Email(email);

        return new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = validatedEmail.Value,
            Phone = phone,
            CompanyName = companyName,
            Segment = segment,
            Classification = classification,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(
        string firstName,
        string lastName,
        string email,
        CustomerSegment segment,
        CustomerClassification classification,
        string? phone = null,
        string? companyName = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required.");

        var validatedEmail = new Email(email);

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = validatedEmail.Value;
        Segment = segment;
        Classification = classification;
        Phone = phone;
        CompanyName = companyName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (_contacts.Any(c => c.IsActive) || _interactions.Any())
            throw new DomainException(
                "Cannot deactivate customer with active contacts or interactions.");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public string FullName => $"{FirstName} {LastName}";
}