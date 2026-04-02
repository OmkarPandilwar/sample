using CustomerManagement.Application.Commands.Contacts;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.Application.Commands;

public class AddContactHandlerTests
{
    private readonly Mock<IContactRepository> _contactRepoMock;
    private readonly Mock<ICustomerRepository> _customerRepoMock;
    private readonly AddContactHandler _handler;

    public AddContactHandlerTests()
    {
        _contactRepoMock = new Mock<IContactRepository>();
        _customerRepoMock = new Mock<ICustomerRepository>();
        _handler = new AddContactHandler(_contactRepoMock.Object, _customerRepoMock.Object);
    }

    // Test 21
    [Fact]
    public async Task Handle_AddPrimaryContact_WhenNoPrimaryExists_ShouldSucceed()
    {
        var customer = Customer.Create(
            "John", "Smith", "john@acme.com",
            CustomerSegment.Corporate, CustomerClassification.Gold);

        _customerRepoMock.Setup(r => r.GetByIdAsync(customer.Id, default))
            .ReturnsAsync(customer);
        _contactRepoMock.Setup(r => r.HasPrimaryContactAsync(customer.Id, default))
            .ReturnsAsync(false);
        _contactRepoMock.Setup(r => r.AddAsync(It.IsAny<Contact>(), default))
            .Returns(Task.CompletedTask);
        _contactRepoMock.Setup(r => r.SaveChangesAsync(default))
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
    public async Task Handle_AddPrimaryContact_WhenPrimaryExists_ShouldThrow()
    {
        var customer = Customer.Create(
            "John", "Smith", "john@acme.com",
            CustomerSegment.Corporate, CustomerClassification.Gold);

        _customerRepoMock.Setup(r => r.GetByIdAsync(customer.Id, default))
            .ReturnsAsync(customer);
        _contactRepoMock.Setup(r => r.HasPrimaryContactAsync(customer.Id, default))
            .ReturnsAsync(true);

        var command = new AddContactCommand(
            customer.Id, "Bob", "Jones",
            "bob@acme.com", null, null, true);

        Func<Task> act = () => _handler.HandleAsync(command);

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*already has a primary contact*");
    }
}