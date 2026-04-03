using CustomerManagement.Application.Commands.Contacts;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.Application.Commands;

public class AddContactHandlerTests
{
    private readonly Mock<IContactRepository> _contactRepositoryMock;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly AddContactHandler _handler;

    public AddContactHandlerTests()
    {
        _contactRepositoryMock = new Mock<IContactRepository>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _handler = new AddContactHandler(_contactRepositoryMock.Object, _customerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddContact_WhenCustomerExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = Customer.Create("John", "Doe", Email.Create("john@example.com"), PhoneNumber.Create("+1234567890"), "Tech Corp", CustomerClassification.Prospect, CustomerType.Individual, CustomerSegment.SMB);
        typeof(Customer).GetProperty("Id")!.SetValue(customer, customerId);

        _customerRepositoryMock.Setup(x => x.GetByIdAsync(customerId)).ReturnsAsync(customer);
        _contactRepositoryMock.Setup(x => x.IsPrimaryContactExistsAsync(customerId)).ReturnsAsync(false);
        _contactRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Contact>())).Returns(Task.CompletedTask);

        var command = new AddContactCommand
        {
            CustomerId = customerId,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane@example.com",
            Phone = "+0987654321",
            IsPrimary = true
        };

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be("Jane");
        _contactRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Contact>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenPrimaryContactExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _contactRepositoryMock.Setup(x => x.IsPrimaryContactExistsAsync(customerId)).ReturnsAsync(true);

        var command = new AddContactCommand
        {
            CustomerId = customerId,
            IsPrimary = true
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }
}