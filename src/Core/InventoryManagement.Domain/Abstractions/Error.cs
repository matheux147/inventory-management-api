namespace InventoryManagement.Domain.Abstractions;

public sealed record Error(string Code)
{
    public static readonly Error None = new(string.Empty);
}