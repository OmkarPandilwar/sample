namespace CustomerManagement.Application.DTOs;

public record HealthScoreDto(
    Guid CustomerId,
    string CustomerName,
    int Score,
    string Status,
    string Color,
    int RecentInteractions,
    decimal AccountValue,
    string Classification,
    DateTime CalculatedAt
);