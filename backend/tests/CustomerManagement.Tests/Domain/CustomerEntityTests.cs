using CustomerManagement.Domain.Entities;
using CustomerManagement.Domain.Enums;
using CustomerManagement.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace CustomerManagement.Tests.Domain;

public class CustomerEntityTests
{
    // Test 1
    [Fact]
    public void Create_ValidCustomer_ShouldSucceed()
    {
        var customer = Customer.Create(
            "John Smith", "john@acme.com",
            CustomerClassification.VIP,
            CustomerType.Business,
            CustomerSegment.Enterprise);

        customer.Should().NotBeNull();
        customer.CustomerName.Should().Be("John Smith");
        customer.Email.Should().Be("john@acme.com");
        customer.IsActive.Should().BeTrue();
    }

    // Test 2
    [Fact]
    public void Create_InvalidEmail_ShouldThrowDomainException()
    {
        Assert.Throws<DomainException>(() => Customer.Create(
            "John Smith", "not-an-email",
            CustomerClassification.VIP,
            CustomerType.Business,
            CustomerSegment.Enterprise));
    }

    // Test 3
    [Fact]
    public void Create_EmptyFirstName_ShouldThrowDomainException()
    {
        Action act = () => Customer.Create(
            "", "john@acme.com",
            CustomerClassification.VIP,
            CustomerType.Business,
            CustomerSegment.Enterprise);

        act.Should().Throw<DomainException>()
            .WithMessage("*Customer name is required*");
    }

    // Test 4
    [Fact]
    public void Update_ValidData_ShouldUpdateFields()
    {
        var customer = Customer.Create(
            "John Smith", "john@acme.com",
            CustomerClassification.VIP,
            CustomerType.Business,
            CustomerSegment.Enterprise);

        customer.Update(
            "Jane Doe", "jane@acme.com",
            CustomerClassification.VIP,
            CustomerType.Business,
            CustomerSegment.Enterprise);

        customer.CustomerName.Should().Be("Jane Doe");
        customer.Email.Should().Be("jane@acme.com");
        customer.Segment.Should().Be(CustomerSegment.Enterprise);
        customer.ModifiedDate.Should().NotBeNull();
    }

    // Test 5
    [Fact]
    public void Deactivate_CustomerWithNoActiveData_ShouldSucceed()
    {
        var customer = Customer.Create(
            "John Smith", "john@acme.com",
            CustomerClassification.VIP,
            CustomerType.Business,
            CustomerSegment.Enterprise);

        customer.Deactivate();

        customer.IsActive.Should().BeFalse();
    }

    [Fact]
    public void CustomerName_ShouldBeCorrect()
    {
        var customer = Customer.Create(
            "John Smith", "john@acme.com",
            CustomerClassification.VIP,
            CustomerType.Business,
            CustomerSegment.Enterprise);

        customer.CustomerName.Should().Be("John Smith");
    }

    // Test 7
    [Fact]
    public void Create_EmailShouldBeLowercase()
    {
        var customer = Customer.Create(
            "John Smith", "JOHN@ACME.COM",
            CustomerClassification.VIP,
            CustomerType.Business,
            CustomerSegment.Enterprise);

        customer.Email.Should().Be("john@acme.com");
    }
}