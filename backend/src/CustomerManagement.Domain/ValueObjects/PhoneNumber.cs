using System.Text.RegularExpressions;
using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Domain.ValueObjects;

public sealed class PhoneNumber
{
    public string Value { get; }

    private static readonly Regex PhoneRegex = new(
        @"^\+?[1-9]\d{7,14}$",
        RegexOptions.Compiled);

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Phone number cannot be empty.");

        var cleaned = Regex.Replace(value, @"[\s\-\(\)]", "");

        if (!PhoneRegex.IsMatch(cleaned))
            throw new DomainException($"'{value}' is not a valid phone number.");

        Value = cleaned;
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is PhoneNumber other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}