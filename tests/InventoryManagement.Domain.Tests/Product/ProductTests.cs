using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Exceptions;
using CategoryShortcode = InventoryManagement.Domain.ValueObjects.CategoryShortcode;
using CountryCode = InventoryManagement.Domain.ValueObjects.CountryCode;
using CurrencyCode = InventoryManagement.Domain.ValueObjects.CurrencyCode;
using Email = InventoryManagement.Domain.ValueObjects.Email;

namespace InventoryManagement.Domain.Tests;

public class ProductTests
{
    private readonly Entities.Supplier _supplier;
    private readonly Entities.Category _category;

    public ProductTests()
    {
        _supplier = Entities.Supplier.Create("Test Supplier", Email.Create("sup@test.com"), CurrencyCode.USD, CountryCode.Create("US"));
        _category = Entities.Category.Create("Test Category", CategoryShortcode.Create("CAT-1"));
    }

    [Fact]
    public void Create_WithValidParameters_ShouldCreateProduct()
    {
        // Arrange
        var description = "Test Product";
        var acqCost = 10m;
        var acqCostUsd = 10m;
        var acquireDate = DateTime.UtcNow;

        // Act
        var product = Entities.Product.Create(_supplier, _category, description, acqCost, acqCostUsd, acquireDate);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(_supplier, product.Supplier);
        Assert.Equal(_supplier.Id, product.SupplierId);
        Assert.Equal(_category, product.Category);
        Assert.Equal(_category.Id, product.CategoryId);
        Assert.Equal(description, product.Description);
        Assert.Equal(acqCost, product.AcquisitionCost);
        Assert.Equal(acqCostUsd, product.AcquisitionCostUsd);
        Assert.Equal(acquireDate, product.AcquireDate);
        Assert.Equal(ProductStatus.Created, product.Status);
        Assert.Null(product.SoldDate);
        Assert.Null(product.CancelDate);
        Assert.Null(product.ReturnDate);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithNullOrEmptyDescription_ShouldThrowDomainException(string description)
    {
        // Arrange & Act
        var exception = Assert.Throws<DomainException>(() => { _ = Entities.Product.Create(_supplier, _category, description, 10m, 10m, DateTime.UtcNow); });

        // Assert
        Assert.Equal(ProductMessages.ProductDescriptionIsRequired, exception.Message);
    }

    [Fact]
    public void ChangeDescription_ShouldUpdateDescription()
    {
        // Arrange
        var product = Entities.Product.Create(_supplier, _category, "Old Description", 10m, 10m, DateTime.UtcNow);
        var newDescription = "New Description";

        // Act
        product.ChangeDescription(newDescription);

        // Assert
        Assert.Equal(newDescription, product.Description);
    }

    [Fact]
    public void ChangeCategory_ShouldUpdateCategory()
    {
        // Arrange
        var product = Entities.Product.Create(_supplier, _category, "Test Product", 10m, 10m, DateTime.UtcNow);
        var newCategory = Entities.Category.Create("New Category", CategoryShortcode.Create("NEW"));

        // Act
        product.ChangeCategory(newCategory);

        // Assert
        Assert.Equal(newCategory, product.Category);
        Assert.Equal(newCategory.Id, product.CategoryId);
    }

    [Fact]
    public void Sell_WithValidDate_ShouldUpdateStatusAndSoldDate()
    {
        // Arrange
        var acquireDate = DateTime.UtcNow.AddDays(-1);
        var soldDate = DateTime.UtcNow;
        var product = Entities.Product.Create(_supplier, _category, "Test", 10m, 10m, acquireDate);

        // Act
        product.Sell(soldDate);

        // Assert
        Assert.Equal(ProductStatus.Sold, product.Status);
        Assert.Equal(soldDate, product.SoldDate);
    }

    [Fact]
    public void Sell_WithDateEarlierThanAcquireDate_ShouldThrowDomainException()
    {
        // Arrange
        var acquireDate = DateTime.UtcNow;
        var soldDate = acquireDate.AddDays(-1);
        var product = Entities.Product.Create(_supplier, _category, "Test", 10m, 10m, acquireDate);

        // Act
        var exception = Assert.Throws<DomainException>(() => { product.Sell(soldDate); });

        // Assert
        Assert.Equal(ProductMessages.SoldDateCannotBeEarlierThanAcquireDate, exception.Message);
    }

    [Fact]
    public void Cancel_WithValidDate_ShouldUpdateStatusAndCancelDate()
    {
        // Arrange
        var acquireDate = DateTime.UtcNow.AddDays(-1);
        var cancelDate = DateTime.UtcNow;
        var product = Entities.Product.Create(_supplier, _category, "Test", 10m, 10m, acquireDate);

        // Act
        product.Cancel(cancelDate);

        // Assert
        Assert.Equal(ProductStatus.Cancelled, product.Status);
        Assert.Equal(cancelDate, product.CancelDate);
    }

    [Fact]
    public void Sell_WhenProductIsCancelled_ShouldThrowDomainException()
    {
        // Arrange
        var acquireDate = DateTime.UtcNow.AddDays(-2);
        var product = Entities.Product.Create(_supplier, _category, "Test", 10m, 10m, acquireDate);
        product.Cancel(DateTime.UtcNow.AddDays(-1));

        // Act
        var exception = Assert.Throws<DomainException>(() => { product.Sell(DateTime.UtcNow); });

        // Assert
        Assert.Equal(ProductMessages.CancelledProductCannotBeSold, exception.Message);
    }

    [Fact]
    public void Sell_WhenProductIsReturned_ShouldThrowDomainException()
    {
        // Arrange
        var acquireDate = DateTime.UtcNow.AddDays(-3);
        var product = Entities.Product.Create(_supplier, _category, "Test", 10m, 10m, acquireDate);
        product.Sell(DateTime.UtcNow.AddDays(-2));
        product.Return(DateTime.UtcNow.AddDays(-1));

        // Act
        var exception = Assert.Throws<DomainException>(() => { product.Sell(DateTime.UtcNow); });

        // Assert
        Assert.Equal(ProductMessages.ReturnedProductCannotBeSold, exception.Message);
    }

    [Fact]
    public void Cancel_WhenProductIsSold_ShouldUpdateStatusAndCancelDate()
    {
        // Arrange
        var acquireDate = DateTime.UtcNow.AddDays(-3);
        var product = Entities.Product.Create(_supplier, _category, "Test", 10m, 10m, acquireDate);
        product.Sell(DateTime.UtcNow.AddDays(-2));
        var cancelDate = DateTime.UtcNow.AddDays(-1);

        // Act
        product.Cancel(cancelDate);

        // Assert
        Assert.Equal(ProductStatus.Cancelled, product.Status);
        Assert.Equal(cancelDate, product.CancelDate);
    }

    [Fact]
    public void Return_WithValidDate_ShouldUpdateStatusAndReturnDate()
    {
        // Arrange
        var acquireDate = DateTime.UtcNow.AddDays(-2);
        var soldDate = DateTime.UtcNow.AddDays(-1);
        var returnDate = DateTime.UtcNow;
        var product = Entities.Product.Create(_supplier, _category, "Test", 10m, 10m, acquireDate);
        product.Sell(soldDate);

        // Act
        product.Return(returnDate);

        // Assert
        Assert.Equal(ProductStatus.Returned, product.Status);
        Assert.Equal(returnDate, product.ReturnDate);
    }
}
