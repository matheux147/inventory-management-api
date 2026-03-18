namespace InventoryManagement.Application.DTOs.Categories;

public sealed record CategoryResponseDto(
    Guid Id,
    string Name,
    string Shortcode,
    Guid? ParentCategoryId);