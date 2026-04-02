namespace CustomerManagement.Application.DTOs;

public record ChurnRiskDto(
    Guid CustomerId,
    string CustomerName,
    string RiskLevel,
    string Color,
    int DaysSinceLastInteraction,
    string Classification,
    decimal AccountValue,
    DateTime CalculatedAt
);