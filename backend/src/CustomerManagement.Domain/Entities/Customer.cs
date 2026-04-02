using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.ValueObjects;

namespace CustomerManagement.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string CustomerName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public string? Website { get; private set; }
    public string? Industry { get; private set; }
    public string? CompanySize { get; private set; }
    public CustomerClassification Classification { get; private set; }
    public CustomerType Type { get; private set; }
    public CustomerSegment Segment { get; private set; }
    public decimal AccountValue { get; private set; }
    public string? AssignedSalesRepId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime? ModifiedDate { get; private set; }

    private readonly List<Contact> _contacts = new();
    private readonly List<Address> _addresses = new();
    private readonly List<Interaction> _interactions = new();

    public IReadOnlyCollection<Contact> Contacts => _contacts.AsReadOnly();
    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();
    public IReadOnlyCollection<Interaction> Interactions => _interactions.AsReadOnly();

    private Customer() { }

    public static Customer Create(
        string customerName,
        string email,
        CustomerClassification classification,
        CustomerType type,
        CustomerSegment segment,
        string? phone = null,
        string? website = null,
        string? industry = null,
        string? companySize = null,
        decimal accountValue = 0,
        string? assignedSalesRepId = null)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new DomainException("Customer name is required.");

        var validatedEmail = new Email(email);

        return new Customer
        {
            Id = Guid.NewGuid(),
            CustomerName = customerName.Trim(),
            Email = validatedEmail.Value,
            Phone = phone,
            Website = website,
            Industry = industry,
            CompanySize = companySize,
            Classification = classification,
            Type = type,
            Segment = segment,
            AccountValue = accountValue,
            AssignedSalesRepId = assignedSalesRepId,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };
    }

    public void Update(
        string customerName,
        string email,
        CustomerClassification classification,
        CustomerType type,
        CustomerSegment segment,
        string? phone = null,
        string? website = null,
        string? industry = null,
        string? companySize = null,
        decimal accountValue = 0,
        string? assignedSalesRepId = null)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new DomainException("Customer name is required.");

        var validatedEmail = new Email(email);

        CustomerName = customerName.Trim();
        Email = validatedEmail.Value;
        Classification = classification;
        Type = type;
        Segment = segment;
        Phone = phone;
        Website = website;
        Industry = industry;
        CompanySize = companySize;
        AccountValue = accountValue;
        AssignedSalesRepId = assignedSalesRepId;
        ModifiedDate = DateTime.UtcNow;
    }

    public void ChangeClassification(CustomerClassification classification)
    {
        Classification = classification;
        ModifiedDate = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (_contacts.Any(c => c.IsActive) || _interactions.Any())
            throw new DomainException(
                "Cannot deactivate customer with active contacts or interactions.");
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
    }
}