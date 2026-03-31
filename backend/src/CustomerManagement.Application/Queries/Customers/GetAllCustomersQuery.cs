namespace CustomerManagement.Application.Queries.Customers;

public record GetAllCustomersQuery(bool ActiveOnly = false);