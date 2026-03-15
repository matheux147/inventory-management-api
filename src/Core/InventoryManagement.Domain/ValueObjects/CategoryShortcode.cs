using System.Text.RegularExpressions;
using InventoryManagement.Domain.Exceptions;

namespace InventoryManagement.Domain.ValueObjects;

public sealed record CategoryShortcode
{
    private static readonly Regex ValidPattern = new("^[A-Z0-9_-]+$", RegexOptions.Compiled);

    public string Value { get; }

    private CategoryShortcode(string value)
    {
        Value = value;
    }

    public static CategoryShortcode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Category shortcode is required.");

        var normalized = value.Trim().ToUpperInvariant();

        if (!ValidPattern.IsMatch(normalized))
            throw new DomainException("Category shortcode must contain only letters, numbers, underscore or hyphen.");

        return new CategoryShortcode(normalized);
    }

    public override string ToString() => Value;

    public static implicit operator string(CategoryShortcode shortcode) => shortcode.Value;
}
