using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Tests;

public class CurrencyCodeTests
{
    [Fact]
    public void Create_WithValidCurrencyCode_ShouldSetProperties()
    {
        // Arrange
        var validCode = "BRL";

        // Act
        var currencyCode = ValueObjects.CurrencyCode.Create(validCode);

        // Assert
        Assert.Equal("BRL", currencyCode.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithNullOrWhiteSpace_ShouldThrowDomainException(string value)
    {
        // Arrange & Act
        var exception = Assert.Throws<DomainException>(() => { _ = ValueObjects.CurrencyCode.Create(value); });

        // Assert
        Assert.Equal(SharedMessages.CurrencyCodeIsRequired, exception.Message);
    }

    [Theory]
    [InlineData("BR")]
    [InlineData("BRLA")]
    [InlineData("123")]
    public void Create_WithInvalidFormat_ShouldThrowDomainException(string value)
    {
        // Arrange & Act
        var exception = Assert.Throws<DomainException>(() => { _ = ValueObjects.CurrencyCode.Create(value); });

        // Assert
        Assert.Equal(SharedMessages.InvalidCurrencyCodeFormat, exception.Message);
    }

    [Fact]
    public void DefaultUSD_ShouldReturnUSD()
    {
        // Arrange & Act
        var usdCode = ValueObjects.CurrencyCode.USD;

        // Assert
        Assert.Equal("USD", usdCode.Value);
    }
}
