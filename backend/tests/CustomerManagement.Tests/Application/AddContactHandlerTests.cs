using CustomerManagement.Application.Commands.Contacts;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace CustomerManagement.Tests.Application;

public class AddContactHandlerTests
{
    private readonly Mock<IContactRepository> _contactMock;
    private readonly Mock<ICustomerRepository> _customerMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<IValidator<AddContactCommand>> _validatorMock;
    private readonly AddContactHandler _handler;

    public AddContactHandlerTests()
    {
        _contactMock = new Mock<IContactRepository>();
        _customerMock = new Mock<ICustomerRepository>();
        _cacheMock = new Mock<IDistributedCache>();
        _validatorMock = new Mock<IValidator<AddContactCommand>>();
        _handler = new AddContactHandler(_contactMock.Object, _customerMock.Object, _cacheMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCustomerNotFound_ShouldThrowException()
    {
        // Arrange
        var command = new AddContactCommand(Guid.NewGuid(), "F", "L", "e@e.com", true, "1", "J");
        _customerMock.Setup(c => c.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Customer)null!);

        // Act
        Func<Task> act = () => _handler.HandleAsync(command);

        // Assert
        await act.Should().ThrowAsync<CustomerNotFoundException>();
    }

    [Fact]
    public async Task Handle_WhenPrimaryContactAdded_ShouldDemoteExistingPrimary()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var command = new AddContactCommand(customerId, "F", "L", "e@e.com", true, "1", "J");
        var existingPrimary = CreateTestContact(customerId, true);

        _customerMock.Setup(c => c.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateTestCustomer(customerId));
        _contactMock.Setup(c => c.GetPrimaryContactAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPrimary);

        // Act
        await _handler.HandleAsync(command);

        // Assert
        existingPrimary.IsPrimary.Should().BeFalse();
        _contactMock.Verify(c => c.Update(existingPrimary), Times.Once);
        _contactMock.Verify(c => c.AddAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()), Times.Once);
        _cacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    private static Domain.Entities.Customer CreateTestCustomer(Guid id) 
        => Domain.Entities.Customer.Create("N", "e@e.com", CustomerClassification.Active, CustomerType.Corporate, CustomerSegment.Enterprise);

    private static Contact CreateTestContact(Guid customerId, bool isPrimary)
        => Contact.Create(customerId, "F", "L", "e@e.com", isPrimary, "1", "J");
}
