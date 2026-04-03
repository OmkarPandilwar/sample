using CustomerManagement.API.Controllers;
using CustomerManagement.Application.Commands.Customers;
using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Queries.Customers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CustomerManagement.Tests.API;

public class CustomersControllerTests
{
    private readonly Mock<GetAllCustomersHandler> _getAllHandlerMock;
    private readonly Mock<GetCustomerByIdHandler> _getByIdHandlerMock;
    private readonly Mock<CreateCustomerHandler> _createHandlerMock;
    private readonly Mock<UpdateCustomerHandler> _updateHandlerMock;
    private readonly Mock<DeleteCustomerHandler> _deleteHandlerMock;
    private readonly CustomersController _controller;

    public CustomersControllerTests()
    {
        _getAllHandlerMock = new Mock<GetAllCustomersHandler>(null!);
        _getByIdHandlerMock = new Mock<GetCustomerByIdHandler>(null!);
        _createHandlerMock = new Mock<CreateCustomerHandler>(null!);
        _updateHandlerMock = new Mock<UpdateCustomerHandler>(null!);
        _deleteHandlerMock = new Mock<DeleteCustomerHandler>(null!);
        _controller = new CustomersController(
            _getAllHandlerMock.Object,
            _getByIdHandlerMock.Object,
            _createHandlerMock.Object,
            _updateHandlerMock.Object,
            _deleteHandlerMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnCustomers()
    {
        // Arrange
        var customers = new List<CustomerDto> { new CustomerDto { Id = Guid.NewGuid(), FirstName = "John" } };
        _getAllHandlerMock.Setup(x => x.Handle(It.IsAny<GetAllCustomersQuery>(), default)).ReturnsAsync(customers);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<CustomerDto>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetById_ShouldReturnCustomer_WhenExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new CustomerDto { Id = customerId, FirstName = "John" };
        _getByIdHandlerMock.Setup(x => x.Handle(It.IsAny<GetCustomerByIdQuery>(), default)).ReturnsAsync(customer);

        // Act
        var result = await _controller.GetById(customerId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<CustomerDto>(okResult.Value);
        returnValue.Id.Should().Be(customerId);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedCustomer()
    {
        // Arrange
        var command = new CreateCustomerCommand { FirstName = "John" };
        var customer = new CustomerDto { Id = Guid.NewGuid(), FirstName = "John" };
        _createHandlerMock.Setup(x => x.Handle(command, default)).ReturnsAsync(customer);

        // Act
        var result = await _controller.Create(command);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<CustomerDto>(createdResult.Value);
        returnValue.FirstName.Should().Be("John");
    }
}