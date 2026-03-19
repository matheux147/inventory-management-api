using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Exceptions;

namespace InventoryManagement.Domain.Tests;

public class EmailTests
{
    [Fact]
    public void Create_WithValidEmail_ShouldSetProperties()
    {
        // Arrange
        var validEmail = "test@example.com";

        // Act
        var email = ValueObjects.Email.Create(validEmail);

        // Assert
        Assert.Equal(validEmail, email.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithNullOrWhiteSpace_ShouldThrowDomainException(string value)
    {
        // Arrange & Act
        var exception = Assert.Throws<DomainException>(() => { _ = ValueObjects.Email.Create(value); });

        // Assert
        Assert.Equal(SharedMessages.EmailIsRequired, exception.Message);
    }

    [Theory]
    [InlineData("testexample.com")]
    [InlineData("test@.com")]
    [InlineData("@example.com")]
    public void Create_WithInvalidFormat_ShouldThrowDomainException(string value)
    {
        // Arrange & Act
        var exception = Assert.Throws<DomainException>(() => { _ = ValueObjects.Email.Create(value); });

        // Assert
        Assert.Equal(SharedMessages.InvalidEmailFormat, exception.Message);
    }

    [Fact]
    public void ToString_ShouldReturnEmailValue()
    {
        // Arrange
        var email = ValueObjects.Email.Create("test@domain.com");

        // Act
        var str = email.ToString();

        // Assert
        Assert.Equal("test@domain.com", str);
    }

    [Fact]
    public void ImplicitOperator_ShouldReturnStringValue()
    {
        // Arrange
        var email = ValueObjects.Email.Create("test@domain.com");

        // Act
        string emailString = email;

        // Assert
        Assert.Equal("test@domain.com", emailString);
    }
}
