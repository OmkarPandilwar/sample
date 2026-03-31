using CustomerManagement.Application.DTOs;

namespace CustomerManagement.Application.Interfaces;

public interface IAnalyticsService
{
    Task<CustomerAnalyticsDto> GetAnalyticsAsync(CancellationToken ct = default);
}