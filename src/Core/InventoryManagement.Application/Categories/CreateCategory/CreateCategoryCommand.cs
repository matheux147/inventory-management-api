using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Categories;

namespace InventoryManagement.Application.Categories.CreateCategory;

public sealed record CreateCategoryCommand(
    string Name,
    string Shortcode,
    Guid? ParentCategoryId) : ICommand<CategoryResponseDto>;