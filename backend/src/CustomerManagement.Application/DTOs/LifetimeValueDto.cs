namespace CustomerManagement.Application.DTOs;

public record LifetimeValueDto(
    Guid CustomerId,
    string CustomerName,
    decimal AccountValue,
    int InteractionsCount,
    decimal LifetimeValue,
    DateTime CalculatedAt
);