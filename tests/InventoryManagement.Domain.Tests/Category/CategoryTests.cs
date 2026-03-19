using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Exceptions;

using CategoryShortcode = InventoryManagement.Domain.ValueObjects.CategoryShortcode;

namespace InventoryManagement.Domain.Tests;

public class CategoryTests
{
    private readonly CategoryShortcode _shortcode = CategoryShortcode.Create("CAT-1");

    [Fact]
    public void Create_WithValidParameters_ShouldCreateCategory()
    {
        // Arrange
        var name = "Electronics";

        // Act
        var category = Entities.Category.Create(name, _shortcode);

        // Assert
        Assert.NotNull(category);
        Assert.Equal(name, category.Name);
        Assert.Equal(_shortcode, category.Shortcode);
        Assert.Null(category.ParentCategory);
        Assert.Null(category.ParentCategoryId);
        Assert.Empty(category.Children);
        Assert.NotEqual(Guid.Empty, category.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithNullOrEmptyName_ShouldThrowDomainException(string name)
    {
        // Arrange & Act
        var exception = Assert.Throws<DomainException>(() => { _ = Entities.Category.Create(name, _shortcode); });

        // Assert
        Assert.Equal(CategoryMessages.CategoryNameIsRequired, exception.Message);
    }

    [Fact]
    public void Rename_ShouldUpdateName()
    {
        // Arrange
        var category = Entities.Category.Create("Old Name", _shortcode);
        var newName = "New Name";

        // Act
        category.Rename(newName);

        // Assert
        Assert.Equal(newName, category.Name);
    }

    [Fact]
    public void ChangeShortcode_ShouldUpdateShortcode()
    {
        // Arrange
        var category = Entities.Category.Create("Old Name", _shortcode);
        var newShortcode = CategoryShortcode.Create("NEW_CAT");

        // Act
        category.ChangeShortcode(newShortcode);

        // Assert
        Assert.Equal(newShortcode, category.Shortcode);
    }

    [Fact]
    public void SetParent_WithValidParent_ShouldUpdateParentAndChildren()
    {
        // Arrange
        var parent = Entities.Category.Create("Parent", CategoryShortcode.Create("PAR"));
        var child = Entities.Category.Create("Child", CategoryShortcode.Create("CHI"));

        // Act
        child.SetParent(parent);

        // Assert
        Assert.Equal(parent, child.ParentCategory);
        Assert.Equal(parent.Id, child.ParentCategoryId);
        Assert.Contains(child, parent.Children);
    }

    [Fact]
    public void SetParent_WithSelf_ShouldThrowDomainException()
    {
        // Arrange
        var category = Entities.Category.Create("Self", _shortcode);

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => { category.SetParent(category); });
        Assert.Equal(CategoryMessages.CategoryCannotBeItsOwnParent, exception.Message);
    }
}
