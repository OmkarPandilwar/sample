using CustomerManagement.Application.Commands.Customers;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace CustomerManagement.Tests.Application;

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

    [Fact]
    public async Task Handle_WhenVIPCustomer_ShouldThrowDomainException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = Domain.Entities.Customer.Create("N", "e@e.com", CustomerClassification.VIP, CustomerType.Corporate, CustomerSegment.Enterprise);
        _repoMock.Setup(r => r.GetByIdWithDetailsAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync(customer);

        // Act
        Func<Task> act = () => _handler.HandleAsync(new DeleteCustomerCommand(customerId));

        // Assert
        await act.Should().ThrowAsync<DomainException>().WithMessage("*VIP*");
    }

    [Fact]
    public async Task Handle_WithValidCustomer_ShouldDeactivateAndInvalidateCache()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = Domain.Entities.Customer.Create("N", "e@e.com", CustomerClassification.Active, CustomerType.Corporate, CustomerSegment.Enterprise);
        _repoMock.Setup(r => r.GetByIdWithDetailsAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync(customer);

        // Act
        await _handler.HandleAsync(new DeleteCustomerCommand(customerId));

        // Assert
        customer.IsActive.Should().BeFalse();
        _repoMock.Verify(r => r.Update(customer), Times.Once);
        _cacheMock.Verify(r => r.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }
}
