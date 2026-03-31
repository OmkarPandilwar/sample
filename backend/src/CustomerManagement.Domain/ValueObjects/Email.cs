using System.Text.RegularExpressions;
using CustomerManagement.Domain.Exceptions;

namespace CustomerManagement.Domain.ValueObjects;

public sealed class Email
{
    public string Value { get; }

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email cannot be empty.");

        if (!EmailRegex.IsMatch(value))
            throw new DomainException($"'{value}' is not a valid email address.");

        Value = value.ToLowerInvariant();
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is Email other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}