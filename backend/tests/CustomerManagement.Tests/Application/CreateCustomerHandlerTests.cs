using CustomerManagement.Application.Commands.Customers;
using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace CustomerManagement.Tests.Application;

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
        _handler = new CreateCustomerHandler(_repoMock.Object, _cacheMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnCreatedCustomer()
    {
        // Arrange
        var command = new CreateCustomerCommand(
            "Name", "test@test.com", "123", "site", "ind", "size",
            CustomerClassification.Active, CustomerType.Corporate, 
            CustomerSegment.Enterprise, 1000, "REP1");

        _repoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.ExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.CustomerName.Should().Be("Name");
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        _cacheMock.Verify(r => r.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task Handle_WhenEmailDuplicate_ShouldThrowDomainException()
    {
        // Arrange
        var command = new CreateCustomerCommand("N", "dup@test.com", "1", "s", "i", "s", CustomerClassification.A, CustomerType.C, CustomerSegment.E, 0, "R");
        _repoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        Func<Task> act = () => _handler.HandleAsync(command);

        // Assert
        await act.Should().ThrowAsync<DomainException>().WithMessage("*already exists*");
    }

    [Fact]
    public async Task Handle_WhenEmailAndCompanyDuplicate_ShouldThrowDomainException()
    {
        // Arrange
        var command = new CreateCustomerCommand("Name", "test@test.com", "1", "s", "i", "s", CustomerClassification.A, CustomerType.C, CustomerSegment.E, 0, "R");
        _repoMock.Setup(r => r.EmailExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.ExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        Func<Task> act = () => _handler.HandleAsync(command);

        // Assert
        await act.Should().ThrowAsync<DomainException>().WithMessage("*already exists*");
    }
}
