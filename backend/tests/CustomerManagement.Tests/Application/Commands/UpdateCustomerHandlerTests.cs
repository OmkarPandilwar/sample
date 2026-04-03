using CustomerManagement.Application.Commands.Customers;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.Application.Commands;

public class UpdateCustomerHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly UpdateCustomerHandler _handler;

    public UpdateCustomerHandlerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _handler = new UpdateCustomerHandler(_customerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateCustomer_WhenExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = Customer.Create("John", "Doe", Email.Create("john@example.com"), PhoneNumber.Create("+1234567890"), "Tech Corp", CustomerClassification.Prospect, CustomerType.Individual, CustomerSegment.SMB);
        typeof(Customer).GetProperty("Id")!.SetValue(customer, customerId);

        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync(customer);
        _customerRepositoryMock.Setup(x => x.UpdateAsync(customer)).Returns(Task.CompletedTask);

        var command = new UpdateCustomerCommand
        {
            Id = customerId,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane@example.com",
            Phone = "+0987654321",
            CompanyName = "New Corp",
            Classification = CustomerClassification.Active,
            Type = CustomerType.Business,
            Segment = CustomerSegment.Enterprise,
            AccountValue = 50000,
            AssignedSalesRepId = 1
        };

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be("Jane");
        _customerRepositoryMock.Verify(x => x.UpdateAsync(customer), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenNotExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync((Customer)null!);

        var command = new UpdateCustomerCommand { Id = customerId };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}