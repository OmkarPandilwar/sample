using CustomerManagement.Application.Queries.Customers;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.Application.Queries;

public class GetAllCustomersHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly GetAllCustomersHandler _handler;

    public GetAllCustomersHandlerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _handler = new GetAllCustomersHandler(_customerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllActiveCustomers()
    {
        // Arrange
        var customers = new List<Customer>
        {
            Customer.Create("John", "Doe", Email.Create("john@example.com"), PhoneNumber.Create("+1234567890"), "Tech Corp", CustomerClassification.Prospect, CustomerType.Individual, CustomerSegment.SMB),
            Customer.Create("Jane", "Smith", Email.Create("jane@example.com"), PhoneNumber.Create("+0987654321"), "Biz Corp", CustomerClassification.Active, CustomerType.Business, CustomerSegment.Enterprise)
        };

        _customerRepositoryMock.Setup(x => x.GetAllAsync(true)).ReturnsAsync(customers);

        var query = new GetAllCustomersQuery { ActiveOnly = true };

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.First().FirstName.Should().Be("John");
    }
}