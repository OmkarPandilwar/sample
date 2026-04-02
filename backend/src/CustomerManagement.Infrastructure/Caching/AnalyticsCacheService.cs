using System.Text;
using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CustomerManagement.Infrastructure.Caching;

public class AnalyticsCacheService : IAnalyticsService
{
    private readonly CustomerManagementDbContext _context;
    private readonly IDistributedCache _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public AnalyticsCacheService(
        CustomerManagementDbContext context,
        IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<CustomerAnalyticsDto> GetAnalyticsAsync(
        CancellationToken ct = default)
        => await GetCachedAsync("analytics_overview",
            () => BuildAnalyticsAsync(ct), ct);

    public async Task<IEnumerable<LifetimeValueDto>> GetLifetimeValueAsync(
        CancellationToken ct = default)
        => await GetCachedAsync("analytics_lifetime",
            () => BuildLifetimeValueAsync(ct), ct);

    public async Task<IEnumerable<HealthScoreDto>> GetHealthScoresAsync(
        CancellationToken ct = default)
        => await GetCachedAsync("analytics_health",
            () => BuildHealthScoresAsync(ct), ct);

    public async Task<SegmentationDto> GetSegmentationAsync(
        CancellationToken ct = default)
        => await GetCachedAsync("analytics_segmentation",
            () => BuildSegmentationAsync(ct), ct);

    public async Task<IEnumerable<ChurnRiskDto>> GetChurnRiskAsync(
        CancellationToken ct = default)
        => await GetCachedAsync("analytics_churn",
            () => BuildChurnRiskAsync(ct), ct);

    private async Task<T> GetCachedAsync<T>(
        string key,
        Func<Task<T>> buildFunc,
        CancellationToken ct)
    {
        try
        {
            var cached = await _cache.GetAsync(key, ct);
            if (cached is not null)
                return JsonConvert.DeserializeObject<T>(
                    Encoding.UTF8.GetString(cached))!;
        }
        catch { }

        var data = await buildFunc();
        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        await _cache.SetAsync(key, bytes,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheDuration
            }, ct);

        return data;
    }

    private async Task<CustomerAnalyticsDto> BuildAnalyticsAsync(
        CancellationToken ct)
    {
        var customers = await _context.Customers
            .AsNoTracking().ToListAsync(ct);
        var interactions = await _context.Interactions
            .AsNoTracking().CountAsync(ct);

        var bySegment = customers
            .GroupBy(c => c.Segment.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        var byClassification = customers
            .GroupBy(c => c.Classification.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        var totalValue = customers.Sum(c => c.AccountValue);
        var avgValue = customers.Any()
            ? (double)totalValue / customers.Count : 0;

        return new CustomerAnalyticsDto(
            TotalCustomers: customers.Count,
            ActiveCustomers: customers.Count(c => c.IsActive),
            InactiveCustomers: customers.Count(c => !c.IsActive),
            CustomersBySegment: bySegment,
            CustomersByClassification: byClassification,
            TotalInteractions: interactions,
            TotalAccountValue: totalValue,
            AverageAccountValue: avgValue,
            GeneratedAt: DateTime.UtcNow);
    }

    private async Task<IEnumerable<LifetimeValueDto>> BuildLifetimeValueAsync(
        CancellationToken ct)
    {
        var customers = await _context.Customers
            .AsNoTracking()
            .Where(c => c.IsActive)
            .ToListAsync(ct);

        var interactionCounts = await _context.Interactions
            .AsNoTracking()
            .GroupBy(i => i.CustomerId)
            .Select(g => new { CustomerId = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        return customers.Select(c =>
        {
            var interactions = interactionCounts
                .FirstOrDefault(i => i.CustomerId == c.Id)?.Count ?? 0;
            var ltv = c.AccountValue * (1 + (decimal)interactions * 0.1m);

            return new LifetimeValueDto(
                c.Id, c.CustomerName,
                c.AccountValue, interactions,
                Math.Round(ltv, 2),
                DateTime.UtcNow);
        }).OrderByDescending(x => x.LifetimeValue);
    }

    private async Task<IEnumerable<HealthScoreDto>> BuildHealthScoresAsync(
        CancellationToken ct)
    {
        var customers = await _context.Customers
            .AsNoTracking()
            .Where(c => c.IsActive)
            .ToListAsync(ct);

        var since30Days = DateTime.UtcNow.AddDays(-30);
        var recentInteractions = await _context.Interactions
            .AsNoTracking()
            .Where(i => i.InteractionDate >= since30Days)
            .GroupBy(i => i.CustomerId)
            .Select(g => new { CustomerId = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        return customers.Select(c =>
        {
            var interactions = recentInteractions
                .FirstOrDefault(i => i.CustomerId == c.Id)?.Count ?? 0;

            var score = CalculateHealthScore(
                interactions, c.AccountValue, c.Classification);
            var (status, color) = GetHealthStatus(score);

            return new HealthScoreDto(
                c.Id, c.CustomerName, score, status, color,
                interactions, c.AccountValue,
                c.Classification.ToString(),
                DateTime.UtcNow);
        }).OrderByDescending(x => x.Score);
    }

    private async Task<SegmentationDto> BuildSegmentationAsync(
        CancellationToken ct)
    {
        var customers = await _context.Customers
            .AsNoTracking().ToListAsync(ct);

        return new SegmentationDto(
            BySegment: customers
                .GroupBy(c => c.Segment.ToString())
                .ToDictionary(g => g.Key, g => g.Count()),
            ByClassification: customers
                .GroupBy(c => c.Classification.ToString())
                .ToDictionary(g => g.Key, g => g.Count()),
            ByType: customers
                .GroupBy(c => c.Type.ToString())
                .ToDictionary(g => g.Key, g => g.Count()),
            Total: customers.Count,
            GeneratedAt: DateTime.UtcNow);
    }

    private async Task<IEnumerable<ChurnRiskDto>> BuildChurnRiskAsync(
        CancellationToken ct)
    {
        var customers = await _context.Customers
            .AsNoTracking()
            .Where(c => c.IsActive)
            .ToListAsync(ct);

        var lastInteractions = await _context.Interactions
            .AsNoTracking()
            .GroupBy(i => i.CustomerId)
            .Select(g => new
            {
                CustomerId = g.Key,
                LastDate = g.Max(i => i.InteractionDate)
            })
            .ToListAsync(ct);

        return customers.Select(c =>
        {
            var last = lastInteractions
                .FirstOrDefault(i => i.CustomerId == c.Id);
            var daysSince = last == null
                ? 999
                : (int)(DateTime.UtcNow - last.LastDate).TotalDays;

            var (risk, color) = GetChurnRisk(daysSince, c.Classification);

            return new ChurnRiskDto(
                c.Id, c.CustomerName, risk, color,
                daysSince, c.Classification.ToString(),
                c.AccountValue, DateTime.UtcNow);
        }).OrderByDescending(x => x.DaysSinceLastInteraction);
    }

    private static int CalculateHealthScore(
        int recentInteractions,
        decimal accountValue,
        Domain.Enums.CustomerClassification classification)
    {
        var score = 0;
        score += Math.Min(recentInteractions * 20, 40);
        score += accountValue switch
        {
            > 100000 => 30,
            > 50000 => 20,
            > 10000 => 10,
            _ => 0
        };
        score += classification switch
        {
            Domain.Enums.CustomerClassification.VIP => 30,
            Domain.Enums.CustomerClassification.Active => 20,
            Domain.Enums.CustomerClassification.Prospect => 10,
            _ => 0
        };
        return Math.Min(score, 100);
    }

    private static (string status, string color) GetHealthStatus(int score)
        => score switch
        {
            >= 70 => ("Healthy", "#22c55e"),
            >= 40 => ("At Risk", "#f59e0b"),
            _ => ("Critical", "#ef4444")
        };

    private static (string risk, string color) GetChurnRisk(
        int daysSince,
        Domain.Enums.CustomerClassification classification)
        => daysSince switch
        {
            > 90 => ("High", "#ef4444"),
            > 30 => ("Medium", "#f59e0b"),
            _ => ("Low", "#22c55e")
        };
}