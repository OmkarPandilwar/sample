using CustomerManagement.Application.Commands.Addresses;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace CustomerManagement.Tests.Application;

public class AddAddressHandlerTests
{
    private readonly Mock<IAddressRepository> _addressMock;
    private readonly Mock<ICustomerRepository> _customerMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<IValidator<AddAddressCommand>> _validatorMock;
    private readonly AddAddressHandler _handler;

    public AddAddressHandlerTests()
    {
        _addressMock = new Mock<IAddressRepository>();
        _customerMock = new Mock<ICustomerRepository>();
        _cacheMock = new Mock<IDistributedCache>();
        _validatorMock = new Mock<IValidator<AddAddressCommand>>();
        _handler = new AddAddressHandler(_addressMock.Object, _customerMock.Object, _cacheMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_FirstAddress_ShouldBePrimaryEvenIfNotRequested()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var command = new AddAddressCommand(customerId, AddressType.Shipping, "S", "C", "S", "P", "C", false);

        _customerMock.Setup(c => c.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Domain.Entities.Customer.Create("N", "e@e.com", CustomerClassification.Active, CustomerType.Corporate, CustomerSegment.Enterprise));
        _addressMock.Setup(a => a.HasAnyAddressAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsPrimary.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithExistingAddresses_ShouldDemoteExistingPrimary()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var command = new AddAddressCommand(customerId, AddressType.Billing, "S", "C", "S", "P", "C", true);

        _customerMock.Setup(c => c.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Domain.Entities.Customer.Create("N", "e@e.com", CustomerClassification.Active, CustomerType.Corporate, CustomerSegment.Enterprise));
        _addressMock.Setup(a => a.HasAnyAddressAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _addressMock.Setup(a => a.GetPrimaryAddressAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Domain.Entities.Address.Create(customerId, AddressType.Shipping, "S1", "C1", "S1", "P1", "C1", true));

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.IsPrimary.Should().BeTrue();
        _addressMock.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }
}
