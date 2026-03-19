using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;
using InventoryManagement.Domain.Entities;

using Email = InventoryManagement.Domain.ValueObjects.Email;
using CurrencyCode = InventoryManagement.Domain.ValueObjects.CurrencyCode;
using CountryCode = InventoryManagement.Domain.ValueObjects.CountryCode;

namespace InventoryManagement.Domain.Tests;

public class SupplierTests
{
    private readonly Email _email = Email.Create("supplier@test.com");
    private readonly CurrencyCode _currency = CurrencyCode.USD;
    private readonly CountryCode _country = CountryCode.Create("US");

    [Fact]
    public void Create_WithValidParameters_ShouldCreateSupplier()
    {
        // Arrange
        var name = "Valid Supplier";

        // Act
        var supplier = Entities.Supplier.Create(name, _email, _currency, _country);

        // Assert
        Assert.NotNull(supplier);
        Assert.Equal(name, supplier.Name);
        Assert.Equal(_email, supplier.Email);
        Assert.Equal(_currency, supplier.Currency);
        Assert.Equal(_country, supplier.Country);
        Assert.NotEqual(Guid.Empty, supplier.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithNullOrEmptyName_ShouldThrowDomainException(string name)
    {
        // Arrange & Act
        var exception = Assert.Throws<DomainException>(() => { _ = Entities.Supplier.Create(name, _email, _currency, _country); });

        // Assert
        Assert.Equal(SupplierMessages.SupplierNameIsRequired, exception.Message);
    }

    [Fact]
    public void Rename_ShouldUpdateName()
    {
        // Arrange
        var supplier = Entities.Supplier.Create("Test Name", _email, _currency, _country);
        var newName = "New Test Name";

        // Act
        supplier.Rename(newName);

        // Assert
        Assert.Equal(newName, supplier.Name);
    }

    [Fact]
    public void ChangeEmail_ShouldUpdateEmail()
    {
        // Arrange
        var supplier = Entities.Supplier.Create("Test Name", _email, _currency, _country);
        var newEmail = Email.Create("new@test.com");

        // Act
        supplier.ChangeEmail(newEmail);

        // Assert
        Assert.Equal(newEmail, supplier.Email);
    }

    [Fact]
    public void ChangeCurrency_ShouldUpdateCurrency()
    {
        // Arrange
        var supplier = Entities.Supplier.Create("Test Name", _email, _currency, _country);
        var newCurrency = CurrencyCode.Create("BRL");

        // Act
        supplier.ChangeCurrency(newCurrency);

        // Assert
        Assert.Equal(newCurrency, supplier.Currency);
    }

    [Fact]
    public void ChangeCountry_ShouldUpdateCountry()
    {
        // Arrange
        var supplier = Entities.Supplier.Create("Test Name", _email, _currency, _country);
        var newCountry = CountryCode.Create("BR");

        // Act
        supplier.ChangeCountry(newCountry);

        // Assert
        Assert.Equal(newCountry, supplier.Country);
    }
}
