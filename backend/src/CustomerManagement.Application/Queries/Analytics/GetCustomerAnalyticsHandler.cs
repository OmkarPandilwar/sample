using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;

namespace CustomerManagement.Application.Queries.Analytics;

public class GetCustomerAnalyticsHandler
{
    private readonly IAnalyticsService _analyticsService;

    public GetCustomerAnalyticsHandler(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    public async Task<CustomerAnalyticsDto> HandleAsync(GetCustomerAnalyticsQuery query, CancellationToken ct = default)
    {
        return await _analyticsService.GetAnalyticsAsync(ct);
    }
}