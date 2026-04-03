using CustomerManagement.Application.Commands.Customers;
using CustomerManagement.Application.DTOs;
using CustomerManagement.Domain.Entities;
using CustomerManagement.Application.Interfaces;
using CustomerManagement.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.Application.Commands;

public class CreateCustomerHandlerTests
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly CreateCustomerHandler _handler;

    public CreateCustomerHandlerTests()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _handler = new CreateCustomerHandler(_customerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateCustomer_WhenValidRequest()
    {
        // Arrange
        var request = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Phone = "+1234567890",
            CompanyName = "Tech Corp",
            Classification = CustomerClassification.Prospect,
            Type = CustomerType.Individual,
            Segment = CustomerSegment.SMB
        };

        _customerRepositoryMock.Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);
        _customerRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.HandleAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        _customerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEmailNotUnique()
    {
        // Arrange
        var request = new CreateCustomerCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "existing@example.com",
            Phone = "+1234567890",
            CompanyName = "Tech Corp",
            Classification = CustomerClassification.Prospect,
            Type = CustomerType.Individual,
            Segment = CustomerSegment.SMB
        };

        _customerRepositoryMock.Setup(x => x.IsEmailUniqueAsync("existing@example.com")).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, CancellationToken.None));
    }
}