using System.Text.RegularExpressions;
using InventoryManagement.Domain.Constants;
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
            throw new DomainException(SharedMessages.CountryCodeIsRequired);

        var normalized = value.Trim().ToUpperInvariant();

        if (!ValidPattern.IsMatch(normalized))
            throw new DomainException(SharedMessages.InvalidCountryCodeFormat);

        return new CountryCode(normalized);
    }

    public override string ToString() => Value;

    public static implicit operator string(CountryCode country) => country.Value;
}
