using CustomerManagement.Application.DTOs;
using CustomerManagement.Infrastructure.Authentication;
using CustomerManagement.API.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.API;

public class AuthControllerTests
{
    private readonly AuthController _controller;
    private readonly UserService _userService;

    public AuthControllerTests()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:SecretKey"] = "CustomerMgmt_SuperSecret_Key_2024_MustBe32CharsLong!",
                ["JwtSettings:Issuer"] = "CustomerManagementAPI",
                ["JwtSettings:Audience"] = "CustomerManagementClient",
                ["JwtSettings:ExpiryMinutes"] = "60"
            })
            .Build();

        _userService = new UserService();
        var tokenService = new JwtTokenService(config);
        var loggerMock = new Mock<ILogger<AuthController>>();
        _controller = new AuthController(tokenService, _userService, loggerMock.Object);
    }

    // Test 39
    [Fact]
    public void Login_ValidAdminCredentials_ShouldReturn200WithToken()
    {
        var request = new LoginRequest("admin", "admin");

        var result = _controller.Login(request);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var response = okResult.Value.Should()
            .BeOfType<LoginResponse>().Subject;
        response.Token.Should().NotBeNullOrEmpty();
        response.Role.Should().Be("Admin");
    }

    // Test 40
    [Fact]
    public void Login_ValidManagerCredentials_ShouldReturn200WithManagerRole()
    {
        var request = new LoginRequest("manager", "Manager@123");

        var result = _controller.Login(request);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should()
            .BeOfType<LoginResponse>().Subject;
        response.Role.Should().Be("SalesManager");
    }

    // Test 41
    [Fact]
    public void Login_InvalidCredentials_ShouldReturn401()
    {
        var request = new LoginRequest("admin", "WrongPassword");

        var result = _controller.Login(request);

        result.Should().BeOfType<UnauthorizedObjectResult>()
            .Which.StatusCode.Should().Be(401);
    }

    // Test 42
    [Fact]
    public void Login_ValidSalesRepCredentials_ShouldReturnSalesRepRole()
    {
        var request = new LoginRequest("salesrep", "Sales@123");

        var result = _controller.Login(request);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should()
            .BeOfType<LoginResponse>().Subject;
        response.Role.Should().Be("SalesRep");
    }
}