using System.Text.RegularExpressions;
using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Exceptions;

namespace InventoryManagement.Domain.ValueObjects;

public sealed record CurrencyCode
{
    private static readonly Regex ValidPattern = new("^[A-Z]{3}$", RegexOptions.Compiled);

    public static CurrencyCode USD => new("USD");

    public string Value { get; }

    private CurrencyCode(string value)
    {
        Value = value;
    }

    public static CurrencyCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(SharedMessages.CurrencyCodeIsRequired);

        var normalized = value.Trim().ToUpperInvariant();

        if (!ValidPattern.IsMatch(normalized))
            throw new DomainException(SharedMessages.InvalidCurrencyCodeFormat);

        return new CurrencyCode(normalized);
    }

    public override string ToString() => Value;

    public static implicit operator string(CurrencyCode currency) => currency.Value;
}
