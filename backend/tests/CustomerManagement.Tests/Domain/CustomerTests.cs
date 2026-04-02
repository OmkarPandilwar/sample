using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;
using FluentAssertions;

namespace CustomerManagement.Tests.Domain;

public class CustomerTests
{
    [Fact]
    public void Create_WithValidData_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var customer = Customer.Create(
            "Test Customer", "test@example.com", CustomerClassification.Active,
            CustomerType.Corporate, CustomerSegment.Enterprise);

        // Assert
        customer.CustomerName.Should().Be("Test Customer");
        customer.Email.Should().Be("test@example.com");
        customer.IsActive.Should().BeTrue();
        customer.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowDomainException()
    {
        // Act
        Action act = () => Customer.Create(
            "", "test@example.com", CustomerClassification.Active,
            CustomerType.Corporate, CustomerSegment.Enterprise);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*required*");
    }

    [Fact]
    public void Create_WithInvalidEmail_ShouldThrowDomainException()
    {
        // Act
        Action act = () => Customer.Create(
            "Name", "invalid-email", CustomerClassification.Active,
            CustomerType.Corporate, CustomerSegment.Enterprise);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("*email*");
    }

    [Fact]
    public void Deactivate_ShouldSetActiveToFalse()
    {
        // Arrange
        var customer = Customer.Create(
            "Name", "test@example.com", CustomerClassification.Active,
            CustomerType.Corporate, CustomerSegment.Enterprise);

        // Act
        customer.Deactivate();

        // Assert
        customer.IsActive.Should().BeFalse();
        customer.ModifiedDate.Should().NotBeNull();
    }

    [Fact]
    public void Update_ShouldUpdateFieldsCorrectly()
    {
        // Arrange
        var customer = Customer.Create(
            "Old Name", "old@example.com", CustomerClassification.Prospect,
            CustomerType.Individual, CustomerSegment.SmallBusiness);

        // Act
        customer.Update(
            "New Name", "new@example.com", CustomerClassification.VIP,
            CustomerType.Corporate, CustomerSegment.Enterprise);

        // Assert
        customer.CustomerName.Should().Be("New Name");
        customer.Email.Should().Be("new@example.com");
        customer.Classification.Should().Be(CustomerClassification.VIP);
    }
}
