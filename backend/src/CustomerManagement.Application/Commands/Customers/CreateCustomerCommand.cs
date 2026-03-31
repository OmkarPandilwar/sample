using CustomerManagement.Application.DTOs;
using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Application.Commands.Customers;

public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? CompanyName,
    CustomerSegment Segment,
    CustomerClassification Classification
);