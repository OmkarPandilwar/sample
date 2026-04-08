using System.Text.Json.Serialization;
using CustomerManagement.Domain.Enums;

namespace CustomerManagement.Application.DTOs;

public record CustomerDto(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("customerName")] string CustomerName,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("phone")] string? Phone,
    [property: JsonPropertyName("website")] string? Website,
    [property: JsonPropertyName("industry")] string? Industry,
    [property: JsonPropertyName("companySize")] string? CompanySize,
    [property: JsonPropertyName("classification")] CustomerClassification Classification,
    [property: JsonPropertyName("type")] CustomerType Type,
    [property: JsonPropertyName("segment")] CustomerSegment Segment,
    [property: JsonPropertyName("accountValue")] decimal AccountValue,
    [property: JsonPropertyName("assignedSalesRepId")] string? AssignedSalesRepId,
    [property: JsonPropertyName("isActive")] bool IsActive,
    [property: JsonPropertyName("createdDate")] DateTime CreatedDate,
    [property: JsonPropertyName("modifiedDate")] DateTime? ModifiedDate
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