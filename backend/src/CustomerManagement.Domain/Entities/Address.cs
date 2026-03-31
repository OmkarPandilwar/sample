using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Domain.Entities;

public class Address
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public string Street { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string PostalCode { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public bool IsPrimary { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation
    public Customer? Customer { get; private set; }

    private Address() { }

    public static Address Create(
        Guid customerId,
        string street,
        string city,
        string state,
        string postalCode,
        string country,
        bool isPrimary = false)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Street is required.");

        if (string.IsNullOrWhiteSpace(city))
            throw new DomainException("City is required.");

        return new Address
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Street = street.Trim(),
            City = city.Trim(),
            State = state.Trim(),
            PostalCode = postalCode.Trim(),
            Country = country.Trim(),
            IsPrimary = isPrimary,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string street, string city, string state, string postalCode, string country)
    {
        Street = street.Trim();
        City = city.Trim();
        State = state.Trim();
        PostalCode = postalCode.Trim();
        Country = country.Trim();
    }
}