using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Application.DTOs;

public record CustomerDto(
    Guid Id,
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
    string? AssignedSalesRepId,
    bool IsActive,
    DateTime CreatedDate,
    DateTime? ModifiedDate
);

public record CreateCustomerRequest(
    string CustomerName,
    string Email,
    string? Phone,
    string? Website,
    string? Industry,
    string? CompanySize,
    CustomerClassification Classification,
    CustomerType Type,
    CustomerSegment Segment,
    decimal AccountValue = 0,
    string? AssignedSalesRepId = null
);

public record UpdateCustomerRequest(
    string CustomerName,
    string Email,
    string? Phone,
    string? Website,
    string? Industry,
    string? CompanySize,
    CustomerClassification Classification,
    CustomerType Type,
    CustomerSegment Segment,
    decimal AccountValue = 0,
    string? AssignedSalesRepId = null
);

public record ChangeClassificationRequest(
    CustomerClassification Classification
);