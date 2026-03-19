using InventoryManagement.Application.Abstractions.Errors;
using InventoryManagement.Application.Categories.CreateCategory;
using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Ports.Context;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.ValueObjects;
using Moq;

namespace InventoryManagement.Application.Tests.Categories;

public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateCategoryCommandHandler _handler;

    public CreateCategoryCommandHandlerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateCategoryCommandHandler(_categoryRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WhenShortcodeExists_ShouldReturnShortcodeAlreadyExistsError()
    {
        // Arrange
        var command = new CreateCategoryCommand("Test", "CAT-1", null);

        _categoryRepositoryMock
            .Setup(x => x.ExistsByShortcodeAsync(It.IsAny<CategoryShortcode>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("category.shortcode_already_exists", result.Error.Code);
        Assert.Equal(ApplicationErrors.CategoryShortcodeAlreadyExists, result.Error);
    }

    [Fact]
    public async Task Handle_WhenParentCategoryDoesNotExist_ShouldReturnCategoryNotFoundError()
    {
        // Arrange
        var parentId = Guid.NewGuid();
        var command = new CreateCategoryCommand("Test", "CAT-1", parentId);

        _categoryRepositoryMock
            .Setup(x => x.ExistsByShortcodeAsync(It.IsAny<CategoryShortcode>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(parentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("category.not_found", result.Error.Code);
        Assert.Equal(ApplicationErrors.CategoryNotFound, result.Error);
    }

    [Fact]
    public async Task Handle_WithValidData_ShouldCreateCategoryAndReturnSuccess()
    {
        // Arrange
        var command = new CreateCategoryCommand("Test", "CAT-1", null);

        _categoryRepositoryMock
            .Setup(x => x.ExistsByShortcodeAsync(It.IsAny<CategoryShortcode>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(command.Name, result.Value.Name);
        Assert.Equal(command.Shortcode, result.Value.Shortcode);
        
        _categoryRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
