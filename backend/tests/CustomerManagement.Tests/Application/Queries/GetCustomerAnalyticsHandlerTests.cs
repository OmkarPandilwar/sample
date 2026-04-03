using CustomerManagement.Application.Queries.Analytics;
using CustomerManagement.Application.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.Application.Queries;

public class GetCustomerAnalyticsHandlerTests
{
    private readonly Mock<IAnalyticsService> _analyticsServiceMock;
    private readonly GetCustomerAnalyticsHandler _handler;

    public GetCustomerAnalyticsHandlerTests()
    {
        _analyticsServiceMock = new Mock<IAnalyticsService>();
        _handler = new GetCustomerAnalyticsHandler(_analyticsServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAnalytics()
    {
        // Arrange
        var analytics = new CustomerManagement.Application.DTOs.AnalyticsDto
        {
            LifetimeValue = 100000,
            HealthScore = 85,
            SegmentationDistribution = new Dictionary<string, int> { { "SMB", 10 }, { "Enterprise", 5 } },
            ChurnRisk = 0.15
        };

        _analyticsServiceMock.Setup(x => x.GetAnalyticsAsync()).ReturnsAsync(analytics);

        var query = new GetCustomerAnalyticsQuery();

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.LifetimeValue.Should().Be(100000);
        result.HealthScore.Should().Be(85);
    }
}