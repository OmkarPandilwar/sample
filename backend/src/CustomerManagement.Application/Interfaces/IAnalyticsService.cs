using CustomerManagement.Application.DTOs;

namespace CustomerManagement.Application.Interfaces;

public interface IAnalyticsService
{
    Task<CustomerAnalyticsDto> GetAnalyticsAsync(CancellationToken ct = default);
    Task<IEnumerable<LifetimeValueDto>> GetLifetimeValueAsync(CancellationToken ct = default);
    Task<IEnumerable<HealthScoreDto>> GetHealthScoresAsync(CancellationToken ct = default);
    Task<SegmentationDto> GetSegmentationAsync(CancellationToken ct = default);
    Task<IEnumerable<ChurnRiskDto>> GetChurnRiskAsync(CancellationToken ct = default);
}