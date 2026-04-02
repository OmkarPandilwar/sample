namespace CustomerManagement.Application.Queries.Customers;

public record GetAllCustomersQuery(
    bool ActiveOnly = false, 
    string? AssignedRepId = null,
    int PageNumber = 1,
    int PageSize = 10);