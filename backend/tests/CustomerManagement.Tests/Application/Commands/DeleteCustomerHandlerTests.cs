using CustomerManagement.Application.Commands.Customers;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.Application.Commands;

public class DeleteCustomerHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly DeleteCustomerHandler _handler;

    public DeleteCustomerHandlerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _handler = new DeleteCustomerHandler(_customerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeactivateCustomer_WhenExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = Customer.Create("John", "Doe", Email.Create("john@example.com"), PhoneNumber.Create("+1234567890"), "Tech Corp", CustomerClassification.Prospect, CustomerType.Individual, CustomerSegment.SMB);
        typeof(Customer).GetProperty("Id")!.SetValue(customer, customerId);

        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync(customer);
        _customerRepositoryMock.Setup(x => x.UpdateAsync(customer)).Returns(Task.CompletedTask);

        var command = new DeleteCustomerCommand { Id = customerId };

        // Act
        await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        customer.IsActive.Should().BeFalse();
        _customerRepositoryMock.Verify(x => x.UpdateAsync(customer), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenNotExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync((Customer)null!);

        var command = new DeleteCustomerCommand { Id = customerId };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}