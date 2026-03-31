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
    private const string CacheKey = "customer_analytics";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    public AnalyticsCacheService(CustomerManagementDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<CustomerAnalyticsDto> GetAnalyticsAsync(CancellationToken ct = default)
    {
        // Try cache first
        var cached = await _cache.GetAsync(CacheKey, ct);
        if (cached is not null)
        {
            var json = Encoding.UTF8.GetString(cached);
            return JsonConvert.DeserializeObject<CustomerAnalyticsDto>(json)!;
        }

        // Build analytics from DB
        var analytics = await BuildAnalyticsAsync(ct);

        // Store in Redis
        var serialized = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(analytics));
        await _cache.SetAsync(CacheKey, serialized,
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheDuration },
            ct);

        return analytics;
    }

    private async Task<CustomerAnalyticsDto> BuildAnalyticsAsync(CancellationToken ct)
    {
        var customers = await _context.Customers.AsNoTracking().ToListAsync(ct);
        var interactions = await _context.Interactions.AsNoTracking().CountAsync(ct);

        var bySegment = customers
            .GroupBy(c => c.Segment.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        var byClassification = customers
            .GroupBy(c => c.Classification.ToString())
            .ToDictionary(g => g.Key, g => g.Count());

        return new CustomerAnalyticsDto(
            TotalCustomers: customers.Count,
            ActiveCustomers: customers.Count(c => c.IsActive),
            InactiveCustomers: customers.Count(c => !c.IsActive),
            CustomersBySegment: bySegment,
            CustomersByClassification: byClassification,
            TotalInteractions: interactions,
            GeneratedAt: DateTime.UtcNow);
    }
}