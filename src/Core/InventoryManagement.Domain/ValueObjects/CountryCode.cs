using System.Text.RegularExpressions;
using InventoryManagement.Domain.Exceptions;

namespace InventoryManagement.Domain.ValueObjects;

public sealed record CountryCode
{
    private static readonly Regex ValidPattern = new("^[A-Z]{2}$", RegexOptions.Compiled);

    public string Value { get; }

    private CountryCode(string value)
    {
        Value = value;
    }

    public static CountryCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Country code is required.");

        var normalized = value.Trim().ToUpperInvariant();

        if (!ValidPattern.IsMatch(normalized))
            throw new DomainException("Country code must be a valid ISO-3166 alpha-2 style code with 2 letters.");

        return new CountryCode(normalized);
    }

    public override string ToString() => Value;

    public static implicit operator string(CountryCode country) => country.Value;
}
