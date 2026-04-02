using CustomerManagement.Application.Commands.Customers;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.Application.Commands;

public class CreateCustomerHandlerTests
{
    private readonly Mock<ICustomerRepository> _repoMock;
    private readonly CreateCustomerHandler _handler;

    public CreateCustomerHandlerTests()
    {
        _repoMock = new Mock<ICustomerRepository>();
        _handler = new CreateCustomerHandler(_repoMock.Object);
    }

    // Test 13
    [Fact]
    public async Task Handle_NewCustomer_ShouldCreateSuccessfully()
    {
        _repoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), default))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Customer>(), default))
            .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(default))
            .ReturnsAsync(1);

        var command = new CreateCustomerCommand(
            "John", "Smith", "john@acme.com",
            null, "Acme Corp",
            CustomerSegment.Corporate,
            CustomerClassification.Gold);

        var result = await _handler.HandleAsync(command);

        result.Should().NotBeNull();
        result.Email.Should().Be("john@acme.com");
        result.FirstName.Should().Be("John");
    }

    // Test 14
    [Fact]
    public async Task Handle_DuplicateEmail_ShouldThrowDomainException()
    {
        _repoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), default))
            .ReturnsAsync(true);

        var command = new CreateCustomerCommand(
            "John", "Smith", "existing@acme.com",
            null, null,
            CustomerSegment.Corporate,
            CustomerClassification.Gold);

        Func<Task> act = () => _handler.HandleAsync(command);

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*already exists*");
    }

    // Test 15
    [Fact]
    public async Task Handle_NewCustomer_ShouldCallSaveChanges()
    {
        _repoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), default))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Customer>(), default))
            .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(default))
            .ReturnsAsync(1);

        var command = new CreateCustomerCommand(
            "Jane", "Doe", "jane@acme.com",
            null, null,
            CustomerSegment.Enterprise,
            CustomerClassification.Platinum);

        await _handler.HandleAsync(command);

        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }
}