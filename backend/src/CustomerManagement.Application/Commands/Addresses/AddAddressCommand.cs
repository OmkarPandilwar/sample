using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Application.Commands.Addresses;

public record AddAddressCommand(
    Guid CustomerId,
    AddressType AddressType,
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsPrimary = false);
