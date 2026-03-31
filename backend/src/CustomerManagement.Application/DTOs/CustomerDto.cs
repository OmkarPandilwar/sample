using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Application.DTOs;

public record CustomerDto(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string? Phone,
    string? CompanyName,
    CustomerSegment Segment,
    CustomerClassification Classification,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateCustomerRequest(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? CompanyName,
    CustomerSegment Segment,
    CustomerClassification Classification
);

public record UpdateCustomerRequest(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? CompanyName,
    CustomerSegment Segment,
    CustomerClassification Classification
);