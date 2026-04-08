using CustomerManagement.Application.Commands.Customers;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.Application.Commands;

public class DeleteCustomerHandlerTests
{
    private readonly Mock<ICustomerRepository> _repoMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly DeleteCustomerHandler _handler;

    public DeleteCustomerHandlerTests()
    {
        _repoMock = new Mock<ICustomerRepository>();
        _cacheMock = new Mock<IDistributedCache>();
        _handler = new DeleteCustomerHandler(_repoMock.Object, _cacheMock.Object);
    }

    // Test 16
    [Fact]
    public async Task Handle_ExistingCustomer_ShouldDeactivate()
    {
        var customer = Customer.Create(
            "John Smith", "john@acme.com",
            CustomerClassification.Active,
            CustomerType.Business,
            CustomerSegment.Enterprise);

        _repoMock.Setup(r => r.GetByIdWithDetailsAsync(customer.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        await _handler.HandleAsync(new DeleteCustomerCommand(customer.Id));

        customer.IsActive.Should().BeFalse();
    }

    // Test 17
    [Fact]
    public async Task Handle_NonExistentCustomer_ShouldThrowNotFoundException()
    {
        var fakeId = Guid.NewGuid();
        _repoMock.Setup(r => r.GetByIdWithDetailsAsync(fakeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer?)null);

        Func<Task> act = () => _handler.HandleAsync(new DeleteCustomerCommand(fakeId));

        await act.Should().ThrowAsync<CustomerNotFoundException>();
    }
}