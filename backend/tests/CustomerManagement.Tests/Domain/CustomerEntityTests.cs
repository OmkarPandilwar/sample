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
            "John", "Smith", "john@acme.com",
            CustomerSegment.Corporate,
            CustomerClassification.Gold);

        customer.Should().NotBeNull();
        customer.FirstName.Should().Be("John");
        customer.Email.Should().Be("john@acme.com");
        customer.IsActive.Should().BeTrue();
    }

    // Test 2
    [Fact]
    public void Create_InvalidEmail_ShouldThrowDomainException()
    {
        Action act = () => Customer.Create(
            "John", "Smith", "not-an-email",
            CustomerSegment.Corporate,
            CustomerClassification.Gold);

        act.Should().Throw<DomainException>()
            .WithMessage("*not a valid email*");
    }

    // Test 3
    [Fact]
    public void Create_EmptyFirstName_ShouldThrowDomainException()
    {
        Action act = () => Customer.Create(
            "", "Smith", "john@acme.com",
            CustomerSegment.Corporate,
            CustomerClassification.Gold);

        act.Should().Throw<DomainException>()
            .WithMessage("*First name is required*");
    }

    // Test 4
    [Fact]
    public void Update_ValidData_ShouldUpdateFields()
    {
        var customer = Customer.Create(
            "John", "Smith", "john@acme.com",
            CustomerSegment.Corporate,
            CustomerClassification.Gold);

        customer.Update(
            "Jane", "Doe", "jane@acme.com",
            CustomerSegment.Enterprise,
            CustomerClassification.Platinum);

        customer.FirstName.Should().Be("Jane");
        customer.Email.Should().Be("jane@acme.com");
        customer.Segment.Should().Be(CustomerSegment.Enterprise);
        customer.UpdatedAt.Should().NotBeNull();
    }

    // Test 5
    [Fact]
    public void Deactivate_CustomerWithNoActiveData_ShouldSucceed()
    {
        var customer = Customer.Create(
            "John", "Smith", "john@acme.com",
            CustomerSegment.Corporate,
            CustomerClassification.Gold);

        customer.Deactivate();

        customer.IsActive.Should().BeFalse();
    }

    // Test 6
    [Fact]
    public void FullName_ShouldCombineFirstAndLastName()
    {
        var customer = Customer.Create(
            "John", "Smith", "john@acme.com",
            CustomerSegment.Corporate,
            CustomerClassification.Gold);

        customer.FullName.Should().Be("John Smith");
    }

    // Test 7
    [Fact]
    public void Create_EmailShouldBeLowercase()
    {
        var customer = Customer.Create(
            "John", "Smith", "JOHN@ACME.COM",
            CustomerSegment.Corporate,
            CustomerClassification.Gold);

        customer.Email.Should().Be("john@acme.com");
    }
}