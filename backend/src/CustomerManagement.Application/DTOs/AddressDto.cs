namespace CustomerManagement.Application.DTOs;

public record AddressDto(
    Guid Id,
    Guid CustomerId,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsPrimary,
    DateTime CreatedAt
);

public record CreateAddressRequest(
    Guid CustomerId,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsPrimary
);