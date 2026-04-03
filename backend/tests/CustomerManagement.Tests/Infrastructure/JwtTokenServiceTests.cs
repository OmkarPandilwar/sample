using CustomerManagement.Infrastructure.Authentication;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace CustomerManagement.Tests.Infrastructure;

public class JwtTokenServiceTests
{
    private readonly JwtTokenService _service;

    public JwtTokenServiceTests()
    {
        var options = Options.Create(new JwtSettings
        {
            SecretKey = "CustomerMgmt_SuperSecret_Key_2024_MustBe32CharsLong!",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryMinutes = 60
        });
        _service = new JwtTokenService(options);
    }

    [Fact]
    public void GenerateToken_ShouldReturnToken_WhenValidUser()
    {
        // Act
        var token = _service.GenerateToken("testuser", "Admin");

        // Assert
        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void ValidateToken_ShouldReturnClaims_WhenValidToken()
    {
        // Arrange
        var token = _service.GenerateToken("testuser", "Admin");

        // Act
        var claims = _service.ValidateToken(token);

        // Assert
        claims.Should().NotBeNull();
        claims.Identity!.Name.Should().Be("testuser");
        claims.FindFirst("role")!.Value.Should().Be("Admin");
    }

    [Fact]
    public void ValidateToken_ShouldReturnNull_WhenInvalidToken()
    {
        // Act
        var claims = _service.ValidateToken("invalidtoken");

        // Assert
        claims.Should().BeNull();
    }
}