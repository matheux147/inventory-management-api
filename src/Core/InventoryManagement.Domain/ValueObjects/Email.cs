using System.Net.Mail;
using InventoryManagement.Domain.Exceptions;

namespace InventoryManagement.Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email is required.");

        var normalized = value.Trim().ToLowerInvariant();

        try
        {
            _ = new MailAddress(normalized);
        }
        catch (FormatException)
        {
            throw new DomainException("Email is invalid.");
        }

        return new Email(normalized);
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}
