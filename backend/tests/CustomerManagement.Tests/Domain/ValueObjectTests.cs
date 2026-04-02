using CustomerManagement.Domain.Exceptions;
using CustomerManagement.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CustomerManagement.Tests.Domain;

public class ValueObjectTests
{
    // Test 8
    [Theory]
    [InlineData("john@example.com")]
    [InlineData("user.name@domain.co")]
    [InlineData("test+tag@gmail.com")]
    public void Email_ValidFormats_ShouldSucceed(string email)
    {
        var emailObj = new Email(email);
        emailObj.Value.Should().NotBeNullOrEmpty();
    }

    // Test 9
    [Theory]
    [InlineData("notanemail")]
    [InlineData("missing@")]
    [InlineData("@nodomain.com")]
    [InlineData("")]
    public void Email_InvalidFormats_ShouldThrow(string email)
    {
        Action act = () => new Email(email);
        act.Should().Throw<DomainException>();
    }

    // Test 10
    [Theory]
    [InlineData("9876543210")]
    [InlineData("+919876543210")]
    [InlineData("14155552671")]
    public void PhoneNumber_ValidFormats_ShouldSucceed(string phone)
    {
        var phoneObj = new PhoneNumber(phone);
        phoneObj.Value.Should().NotBeNullOrEmpty();
    }

    // Test 11
    [Theory]
    [InlineData("123")]
    [InlineData("abcdefghij")]
    [InlineData("")]
    public void PhoneNumber_InvalidFormats_ShouldThrow(string phone)
    {
        Action act = () => new PhoneNumber(phone);
        act.Should().Throw<DomainException>();
    }

    // Test 12
    [Fact]
    public void Email_Equality_SameValueShouldBeEqual()
    {
        var email1 = new Email("john@acme.com");
        var email2 = new Email("JOHN@ACME.COM");

        email1.Should().Be(email2);
    }
}