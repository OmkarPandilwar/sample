using System.Text.Json.Serialization;

namespace CustomerManagement.Application.DTOs;

public record CustomerAnalyticsDto(
    [property: JsonPropertyName("totalCustomers")] int TotalCustomers,
    [property: JsonPropertyName("activeCustomers")] int ActiveCustomers,
    [property: JsonPropertyName("inactiveCustomers")] int InactiveCustomers,
    [property: JsonPropertyName("customersBySegment")] Dictionary<string, int> CustomersBySegment,
    [property: JsonPropertyName("customersByClassification")] Dictionary<string, int> CustomersByClassification,
    [property: JsonPropertyName("totalInteractions")] int TotalInteractions,
    [property: JsonPropertyName("totalAccountValue")] decimal TotalAccountValue,
    [property: JsonPropertyName("averageAccountValue")] double AverageAccountValue,
    [property: JsonPropertyName("generatedAt")] DateTime GeneratedAt
);