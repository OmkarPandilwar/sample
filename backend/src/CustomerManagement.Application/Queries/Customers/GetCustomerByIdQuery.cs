namespace CustomerManagement.Application.Queries.Customers;

public record GetCustomerByIdQuery(
    Guid Id,
    string? AssignedRepId = null);