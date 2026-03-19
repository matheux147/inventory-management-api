using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Tests;

public class CountryCodeTests
{
    [Fact]
    public void Create_WithValidCountryCode_ShouldSetProperties()
    {
        // Arrange
        var validCode = "US";

        // Act
        var countryCode = ValueObjects.CountryCode.Create(validCode);

        // Assert
        Assert.Equal("US", countryCode.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithNullOrWhiteSpace_ShouldThrowDomainException(string value)
    {
        // Arrange & Act
        var exception = Assert.Throws<DomainException>(() => { _ = ValueObjects.CountryCode.Create(value); });

        // Assert
        Assert.Equal(SharedMessages.CountryCodeIsRequired, exception.Message);
    }

    [Theory]
    [InlineData("USA")]
    [InlineData("12")]
    [InlineData("U1")]
    [InlineData("u2")]
    public void Create_WithInvalidFormat_ShouldThrowDomainException(string value)
    {
        // Arrange & Act
        var exception = Assert.Throws<DomainException>(() => { _ = ValueObjects.CountryCode.Create(value); });

        // Assert
        Assert.Equal(SharedMessages.InvalidCountryCodeFormat, exception.Message);
    }

    [Fact]
    public void ToString_ShouldReturnCodeValue()
    {
        // Arrange
        var countryCode = ValueObjects.CountryCode.Create("BR");

        // Act
        var str = countryCode.ToString();

        // Assert
        Assert.Equal("BR", str);
    }
}
