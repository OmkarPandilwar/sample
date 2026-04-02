using CustomerManagement.Domain.Enums;
using CustomerManagement.Infrastructure.Persistence;
using CustomerManagement.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Tests.Infrastructure;

public class CustomerRepositoryTests
{
    private readonly CustomerManagementDbContext _context;
    private readonly CustomerRepository _repository;

    public CustomerRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<CustomerManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CustomerManagementDbContext(options);
        _repository = new CustomerRepository(_context);
    }

    [Fact]
    public async Task GetAllAsync_WhenSalesRep_ShouldOnlyReturnAssignedCustomers()
    {
        // Arrange
        var customer1 = CreateCustomer("C1", "e1@test.com", "REP_001");
        var customer2 = CreateCustomer("C2", "e2@test.com", "REP_002");
        _context.Customers.AddRange(customer1, customer2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync("REP_001");

        // Assert
        result.Should().HaveCount(1);
        result.First().AssignedSalesRepId.Should().Be("REP_001");
    }

    [Fact]
    public async Task GetAllAsync_WhenAdmin_ShouldReturnAllCustomers()
    {
        // Arrange
        var customer1 = CreateCustomer("C1", "e1@test.com", "REP_001");
        var customer2 = CreateCustomer("C2", "e2@test.com", "REP_002");
        _context.Customers.AddRange(customer1, customer2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync(null);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetBySegmentAsync_ShouldFilterCorrectly()
    {
        // Arrange
        var customer1 = CreateCustomer("C1", "e1@test.com", "REP1", CustomerSegment.Enterprise);
        var customer2 = CreateCustomer("C2", "e2@test.com", "REP1", CustomerSegment.SmallBusiness);
        _context.Customers.AddRange(customer1, customer2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetBySegmentAsync(CustomerSegment.Enterprise);

        // Assert
        result.Should().HaveCount(1);
        result.First().Segment.Should().Be(CustomerSegment.Enterprise);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenMatchFound()
    {
        // Arrange
        var customer = CreateCustomer("ExactName", "exact@test.com", "REP1");
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync("exact@test.com", "ExactName");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenNoMatch()
    {
        // Act
        var result = await _repository.ExistsAsync("nonexistent@test.com", "Nonexistent");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task EmailExistsAsync_ShouldReturnTrue_CaseInsensitive()
    {
        // Arrange
        var customer = CreateCustomer("N", "CASE@TEST.COM", "R");
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.EmailExistsAsync("case@test.com");

        // Assert
        result.Should().BeTrue();
    }

    private static Domain.Entities.Customer CreateCustomer(string name, string email, string repId, CustomerSegment segment = CustomerSegment.Enterprise)
        => Domain.Entities.Customer.Create(name, email, CustomerClassification.Active, CustomerType.Corporate, segment, "1", "s", "i", "s", 0, repId);
}
