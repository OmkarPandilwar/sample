namespace CustomerManagement.Application.Commands.Contacts;

public record AddContactCommand(
    Guid CustomerId,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? JobTitle,
    bool IsPrimary
);