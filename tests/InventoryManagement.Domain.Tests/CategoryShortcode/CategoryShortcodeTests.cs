using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Tests;

public class CategoryShortcodeTests
{
    [Fact]
    public void Create_WithValidShortcode_ShouldSetProperties()
    {
        // Arrange
        var validShortcode = "CAT-01_ABC";

        // Act
        var shortcode = ValueObjects.CategoryShortcode.Create(validShortcode);

        // Assert
        Assert.Equal(validShortcode, shortcode.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithNullOrWhiteSpace_ShouldThrowDomainException(string value)
    {
        // Arrange & Act
        var exception = Assert.Throws<DomainException>(() => { _ = ValueObjects.CategoryShortcode.Create(value); });

        // Assert
        Assert.Equal(SharedMessages.CategoryShortcodeIsRequired, exception.Message);
    }

    [Theory]
    [InlineData("CAT#1")]
    [InlineData("CAT 01")]
    [InlineData("CAT@")]
    public void Create_WithInvalidFormat_ShouldThrowDomainException(string value)
    {
        // Arrange & Act
        var exception = Assert.Throws<DomainException>(() => { _ = ValueObjects.CategoryShortcode.Create(value); });

        // Assert
        Assert.Equal(SharedMessages.InvalidCategoryShortcodeFormat, exception.Message);
    }
}
