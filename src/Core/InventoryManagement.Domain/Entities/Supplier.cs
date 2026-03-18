using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Entities;

public class Supplier : Entity
{
    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public CurrencyCode Currency { get; private set; } = null!;
    public CountryCode Country { get; private set; } = null!;

    private Supplier()
    {
    }

    public static Supplier Create(
        string name,
        Email email,
        CurrencyCode currency,
        CountryCode country)
    {
        return new Supplier(
            name,
            email,
            currency,
            country);
    }

    private Supplier(
        string name,
        Email email,
        CurrencyCode currency,
        CountryCode country)
    {
        SetName(name);
        SetEmail(email);
        SetCurrency(currency);
        SetCountry(country);
    }

    public void Rename(string name)
    {
        SetName(name);
    }

    public void ChangeEmail(Email email)
    {
        SetEmail(email);
    }

    public void ChangeCurrency(CurrencyCode currency)
    {
        SetCurrency(currency);
    }

    public void ChangeCountry(CountryCode country)
    {
        SetCountry(country);
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(SupplierMessages.SupplierNameIsRequired);

        Name = name.Trim();
    }

    private void SetEmail(Email email)
    {
        Email = email ?? throw new DomainException(SupplierMessages.SupplierEmailIsRequired);
    }

    private void SetCurrency(CurrencyCode currency)
    {
        Currency = currency ?? throw new DomainException(SupplierMessages.SupplierCurrencyIsRequired);
    }

    private void SetCountry(CountryCode country)
    {
        Country = country ?? throw new DomainException(SupplierMessages.SupplierCountryIsRequired);
    }
}
