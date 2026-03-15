using InventoryManagement.Domain.Exceptions;

namespace InventoryManagement.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public CurrencyCode Currency { get; }

    private Money(decimal amount, CurrencyCode currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, CurrencyCode currency)
    {
        if (amount < 0)
            throw new DomainException("Amount cannot be negative.");

        if (currency is null)
            throw new DomainException("Currency is required.");

        return new Money(amount, currency);
    }

    public override string ToString() => $"{Amount:0.00} {Currency}";
}
