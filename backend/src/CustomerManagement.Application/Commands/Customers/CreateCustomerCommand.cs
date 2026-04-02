using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Application.Commands.Customers;

public record CreateCustomerCommand(
    string CustomerName,
    string Email,
    string? Phone,
    string? Website,
    string? Industry,
    string? CompanySize,
    CustomerClassification Classification,
    CustomerType Type,
    CustomerSegment Segment,
    decimal AccountValue,
    string? AssignedSalesRepId
);