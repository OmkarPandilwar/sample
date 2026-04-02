using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Application.DTOs;

public record AddressDto(
    Guid Id,
    Guid CustomerId,
    AddressType AddressType,
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
    AddressType AddressType,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsPrimary = false
);

public record UpdateAddressRequest(
    AddressType AddressType,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country
);