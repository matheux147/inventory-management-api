using InventoryManagement.Application.Abstractions.Errors;
using InventoryManagement.Application.Categories.DeleteCategory;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Ports.Context;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.ValueObjects;
using Moq;

namespace InventoryManagement.Application.Tests.Categories;

public class DeleteCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteCategoryCommandHandler _handler;

    public DeleteCategoryCommandHandlerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteCategoryCommandHandler(
            _categoryRepositoryMock.Object,
            _productRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ShouldReturnCategoryNotFoundError()
    {
        // Arrange
        var command = new DeleteCategoryCommand(Guid.NewGuid());

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("category.not_found", result.Error.Code);
    }

    [Fact]
    public async Task Handle_WhenCategoryHasChildren_ShouldReturnCategoryHasChildrenError()
    {
        // Arrange
        var command = new DeleteCategoryCommand(Guid.NewGuid());
        var category = Category.Create("Test", CategoryShortcode.Create("CAT-1"));

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _categoryRepositoryMock
            .Setup(x => x.HasChildrenAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("category.has_children", result.Error.Code);
    }

    [Fact]
    public async Task Handle_WhenCategoryHasProducts_ShouldReturnCategoryHasProductsError()
    {
        // Arrange
        var command = new DeleteCategoryCommand(Guid.NewGuid());
        var category = Category.Create("Test", CategoryShortcode.Create("CAT-1"));

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _categoryRepositoryMock
            .Setup(x => x.HasChildrenAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _productRepositoryMock
            .Setup(x => x.ExistsByCategoryIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("category.has_products", result.Error.Code);
    }

    [Fact]
    public async Task Handle_WhenDeletable_ShouldDeleteCategoryAndReturnSuccess()
    {
        // Arrange
        var command = new DeleteCategoryCommand(Guid.NewGuid());
        var category = Category.Create("Test", CategoryShortcode.Create("CAT-1"));

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _categoryRepositoryMock
            .Setup(x => x.HasChildrenAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _productRepositoryMock
            .Setup(x => x.ExistsByCategoryIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        
        _categoryRepositoryMock.Verify(x => x.Delete(category), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
