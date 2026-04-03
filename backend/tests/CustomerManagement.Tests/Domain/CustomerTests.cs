using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CustomerManagement.Tests.Domain;

public class CustomerTests
{
    [Fact]
    public void Create_ShouldCreateCustomer_WhenValidData()
    {
        // Arrange
        var email = Email.Create("test@example.com");
        var phone = PhoneNumber.Create("+1234567890");

        // Act
        var customer = Customer.Create("John", "Doe", email, phone, "Tech Corp", CustomerClassification.Prospect, CustomerType.Individual, CustomerSegment.SMB);

        // Assert
        customer.Should().NotBeNull();
        customer.FirstName.Should().Be("John");
        customer.LastName.Should().Be("Doe");
        customer.Email.Should().Be(email);
        customer.Phone.Should().Be(phone);
        customer.CompanyName.Should().Be("Tech Corp");
        customer.Classification.Should().Be(CustomerClassification.Prospect);
        customer.Type.Should().Be(CustomerType.Individual);
        customer.Segment.Should().Be(CustomerSegment.SMB);
        customer.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_ShouldThrowException_WhenEmailIsNull()
    {
        // Arrange
        PhoneNumber phone = PhoneNumber.Create("+1234567890");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Customer.Create("John", "Doe", null!, phone, "Tech Corp", CustomerClassification.Prospect, CustomerType.Individual, CustomerSegment.SMB));
    }

    [Fact]
    public void Update_ShouldUpdateCustomerDetails()
    {
        // Arrange
        var customer = Customer.Create("John", "Doe", Email.Create("test@example.com"), PhoneNumber.Create("+1234567890"), "Tech Corp", CustomerClassification.Prospect, CustomerType.Individual, CustomerSegment.SMB);
        var newEmail = Email.Create("new@example.com");

        // Act
        customer.Update("Jane", "Smith", newEmail, PhoneNumber.Create("+0987654321"), "New Corp", CustomerClassification.Active, CustomerType.Business, CustomerSegment.Enterprise, 10000, 1);

        // Assert
        customer.FirstName.Should().Be("Jane");
        customer.LastName.Should().Be("Smith");
        customer.Email.Should().Be(newEmail);
        customer.CompanyName.Should().Be("New Corp");
        customer.Classification.Should().Be(CustomerClassification.Active);
        customer.Type.Should().Be(CustomerType.Business);
        customer.Segment.Should().Be(CustomerSegment.Enterprise);
        customer.AccountValue.Should().Be(10000);
        customer.AssignedSalesRepId.Should().Be(1);
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var customer = Customer.Create("John", "Doe", Email.Create("test@example.com"), PhoneNumber.Create("+1234567890"), "Tech Corp", CustomerClassification.Prospect, CustomerType.Individual, CustomerSegment.SMB);

        // Act
        customer.Deactivate();

        // Assert
        customer.IsActive.Should().BeFalse();
    }
}