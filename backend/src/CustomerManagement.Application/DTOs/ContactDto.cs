namespace CustomerManagement.Application.DTOs;

public record ContactDto(
    Guid Id,
    Guid CustomerId,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string? Phone,
    string? JobTitle,
    bool IsPrimary,
    bool IsActive,
    DateTime CreatedAt
);

public record CreateContactRequest(
    Guid CustomerId,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? JobTitle,
    bool IsPrimary
);