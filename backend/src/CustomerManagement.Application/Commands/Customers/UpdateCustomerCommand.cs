using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Application.Commands.Customers;

public record UpdateCustomerCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? CompanyName,
    CustomerSegment Segment,
    CustomerClassification Classification
);