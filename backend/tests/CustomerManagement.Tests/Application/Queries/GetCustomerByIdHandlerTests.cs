using CustomerManagement.Application.Queries.Customers;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.Application.Queries;

public class GetCustomerByIdHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly GetCustomerByIdHandler _handler;

    public GetCustomerByIdHandlerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _handler = new GetCustomerByIdHandler(_customerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCustomer_WhenExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = Customer.Create("John", "Doe", Email.Create("john@example.com"), PhoneNumber.Create("+1234567890"), "Tech Corp", CustomerClassification.Prospect, CustomerType.Individual, CustomerSegment.SMB);
        typeof(Customer).GetProperty("Id")!.SetValue(customer, customerId);

        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync(customer);

        var query = new GetCustomerByIdQuery { Id = customerId };

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(customerId);
        result.FirstName.Should().Be("John");
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync((Customer)null!);

        var query = new GetCustomerByIdQuery { Id = customerId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}