using CustomerManagement.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CustomerManagement.Tests.Domain;

public class EmailTests
{
    [Fact]
    public void Create_ShouldCreateEmail_WhenValid()
    {
        // Act
        var email = Email.Create("test@example.com");

        // Assert
        email.Value.Should().Be("test@example.com");
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("invalid@")]
    [InlineData("@invalid.com")]
    [InlineData("")]
    [InlineData(null)]
    public void Create_ShouldThrowException_WhenInvalid(string invalidEmail)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Email.Create(invalidEmail));
    }
}

public class PhoneNumberTests
{
    [Fact]
    public void Create_ShouldCreatePhoneNumber_WhenValid()
    {
        // Act
        var phone = PhoneNumber.Create("+1234567890");

        // Assert
        phone.Value.Should().Be("+1234567890");
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("123")]
    [InlineData("")]
    [InlineData(null)]
    public void Create_ShouldThrowException_WhenInvalid(string invalidPhone)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PhoneNumber.Create(invalidPhone));
    }
}