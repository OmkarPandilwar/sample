using CustomerManagement.Application.Interfaces;
using CustomerManagement.Application.Queries.Customers;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.Application.Queries;

public class GetAllCustomersHandlerTests
{
    private readonly Mock<ICustomerRepository> _repoMock;
    private readonly GetAllCustomersHandler _handler;

    public GetAllCustomersHandlerTests()
    {
        _repoMock = new Mock<ICustomerRepository>();
        _handler = new GetAllCustomersHandler(_repoMock.Object);
    }

    // Test 18
    [Fact]
    public async Task Handle_ReturnsAllCustomers()
    {
        var customers = new List<Customer>
        {
            Customer.Create("John", "Smith", "john@acme.com",
                CustomerSegment.Corporate, CustomerClassification.Gold),
            Customer.Create("Jane", "Doe", "jane@techcorp.com",
                CustomerSegment.Enterprise, CustomerClassification.Platinum)
        };

        _repoMock.Setup(r => r.GetAllAsync(default))
            .ReturnsAsync(customers);

        var result = await _handler.HandleAsync(new GetAllCustomersQuery());

        result.Should().HaveCount(2);
    }

    // Test 19
    [Fact]
    public async Task Handle_ActiveOnlyFilter_ReturnsOnlyActiveCustomers()
    {
        var activeCustomer = Customer.Create(
            "John", "Smith", "john@acme.com",
            CustomerSegment.Corporate, CustomerClassification.Gold);

        var inactiveCustomer = Customer.Create(
            "Jane", "Doe", "jane@acme.com",
            CustomerSegment.Enterprise, CustomerClassification.Bronze);
        inactiveCustomer.Deactivate();

        _repoMock.Setup(r => r.GetAllAsync(default))
            .ReturnsAsync(new List<Customer> { activeCustomer, inactiveCustomer });

        var result = await _handler.HandleAsync(new GetAllCustomersQuery(ActiveOnly: true));

        result.Should().HaveCount(1);
        result.First().IsActive.Should().BeTrue();
    }

    // Test 20
    [Fact]
    public async Task Handle_EmptyDatabase_ReturnsEmptyList()
    {
        _repoMock.Setup(r => r.GetAllAsync(default))
            .ReturnsAsync(new List<Customer>());

        var result = await _handler.HandleAsync(new GetAllCustomersQuery());

        result.Should().BeEmpty();
    }
}