namespace CustomerManagement.Application.DTOs;

public record CustomerAnalyticsDto(
    int TotalCustomers,
    int ActiveCustomers,
    int InactiveCustomers,
    Dictionary<string, int> CustomersBySegment,
    Dictionary<string, int> CustomersByClassification,
    int TotalInteractions,
    DateTime GeneratedAt
);