using CustomerManagement.Application.Commands.Interactions;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Application.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.Application.Commands;

public class LogInteractionHandlerTests
{
    private readonly Mock<IInteractionRepository> _interactionRepositoryMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly LogInteractionHandler _handler;

    public LogInteractionHandlerTests()
    {
        _interactionRepositoryMock = new Mock<IInteractionRepository>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _handler = new LogInteractionHandler(_interactionRepositoryMock.Object, _customerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldLogInteraction_WhenCustomerExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = Customer.Create("John", "Doe", Email.Create("john@example.com"), PhoneNumber.Create("+1234567890"), "Tech Corp", CustomerClassification.Prospect, CustomerType.Individual, CustomerSegment.SMB);
        typeof(Customer).GetProperty("Id")!.SetValue(customer, customerId);

        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync(customer);
        _interactionRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Interaction>())).Returns(Task.CompletedTask);

        var command = new LogInteractionCommand
        {
            CustomerId = customerId,
            InteractionType = InteractionType.Call,
            Notes = "Called customer",
            CreatedBy = "salesrep"
        };

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.InteractionType.Should().Be(InteractionType.Call);
        _interactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Interaction>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenCustomerNotExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync((Customer)null!);

        var command = new LogInteractionCommand { CustomerId = customerId };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}