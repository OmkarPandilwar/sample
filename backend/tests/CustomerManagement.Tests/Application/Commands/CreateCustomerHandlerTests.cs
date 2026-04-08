using CustomerManagement.Application.Commands.Customers;
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

public class CreateCustomerHandlerTests
{
    private readonly Mock<ICustomerRepository> _repoMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<IValidator<CreateCustomerCommand>> _validatorMock;
    private readonly CreateCustomerHandler _handler;

    public CreateCustomerHandlerTests()
    {
        _repoMock = new Mock<ICustomerRepository>();
        _cacheMock = new Mock<IDistributedCache>();
        _validatorMock = new Mock<IValidator<CreateCustomerCommand>>();
        
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _handler = new CreateCustomerHandler(_repoMock.Object, _cacheMock.Object, _validatorMock.Object);
    }

    // Test 13
    [Fact]
    public async Task Handle_NewCustomer_ShouldCreateSuccessfully()
    {
        _repoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var command = new CreateCustomerCommand(
            "John Smith", "john@acme.com",
            null, null, null, null,
            CustomerClassification.VIP,
            CustomerType.Business,
            CustomerSegment.Enterprise,
            0, null);

        var result = await _handler.HandleAsync(command);

        result.Should().NotBeNull();
        result.Email.Should().Be("john@acme.com");
        result.CustomerName.Should().Be("John Smith");
    }

    // Test 14
    [Fact]
    public async Task Handle_DuplicateEmail_ShouldThrowDomainException()
    {
        _repoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new CreateCustomerCommand(
            "John Smith", "existing@acme.com",
            null, null, null, null,
            CustomerClassification.VIP,
            CustomerType.Business,
            CustomerSegment.Enterprise,
            0, null);

        var act = () => _handler.HandleAsync(command);

        await act.Should().ThrowAsync<DomainException>();
    }

    // Test 15
    [Fact]
    public async Task Handle_NewCustomer_ShouldCallSaveChanges()
    {
        _repoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var command = new CreateCustomerCommand(
            "Jane Doe", "jane@acme.com",
            null, null, null, null,
            CustomerClassification.VIP,
            CustomerType.Business,
            CustomerSegment.Enterprise,
            0, null);

        await _handler.HandleAsync(command);

        _repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}