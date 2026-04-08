using CustomerManagement.Application.Commands.Contacts;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.Application.Commands;

public class AddContactHandlerTests
{
    private readonly Mock<IContactRepository> _contactRepoMock;
    private readonly Mock<ICustomerRepository> _customerRepoMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<IValidator<AddContactCommand>> _validatorMock;
    private readonly AddContactHandler _handler;

    public AddContactHandlerTests()
    {
        _contactRepoMock = new Mock<IContactRepository>();
        _customerRepoMock = new Mock<ICustomerRepository>();
        _cacheMock = new Mock<IDistributedCache>();
        _validatorMock = new Mock<IValidator<AddContactCommand>>();
        
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AddContactCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _handler = new AddContactHandler(_contactRepoMock.Object, _customerRepoMock.Object, _cacheMock.Object, _validatorMock.Object);
    }

    // Test 21
    [Fact]
    public async Task Handle_AddPrimaryContact_WhenNoPrimaryExists_ShouldSucceed()
    {
        var customer = Customer.Create(
            "John Smith", "john@acme.com",
            CustomerClassification.VIP, CustomerType.Business, CustomerSegment.Enterprise);

        _customerRepoMock.Setup(r => r.GetByIdAsync(customer.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);
        _contactRepoMock.Setup(r => r.HasPrimaryContactAsync(customer.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _contactRepoMock.Setup(r => r.AddAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _contactRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var command = new AddContactCommand(
            customer.Id, "Alice", "Brown",
            "alice@acme.com", null, "Manager", true);

        var result = await _handler.HandleAsync(command);

        result.Should().NotBeNull();
        result.IsPrimary.Should().BeTrue();
    }

    // Test 22
    [Fact]
    public async Task Handle_AddPrimaryContact_WhenPrimaryExists_ShouldUpdateExistingPrimary()
    {
        var customer = Customer.Create(
            "John Smith", "john@acme.com",
            CustomerClassification.VIP, CustomerType.Business, CustomerSegment.Enterprise);

        var existingPrimary = Contact.Create(customer.Id, "Old", "Primary", "old@acme.com", true);

        _customerRepoMock.Setup(r => r.GetByIdAsync(customer.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);
        _contactRepoMock.Setup(r => r.GetPrimaryContactAsync(customer.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPrimary);
        _contactRepoMock.Setup(r => r.AddAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _contactRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var command = new AddContactCommand(
            customer.Id, "Bob", "Jones",
            "bob@acme.com", null, null, true);

        var result = await _handler.HandleAsync(command);

        result.Should().NotBeNull();
        result.IsPrimary.Should().BeTrue();
        existingPrimary.IsPrimary.Should().BeFalse(); // Verified update logic
    }
}