using InventoryManagement.Application.DTOs.Categories;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Categories.CreateCategory;

public static class CreateCategoryMapper
{
    public static CategoryResponseDto Map(this Category category)
    {
        return new CategoryResponseDto(
            category.Id,
            category.Name,
            category.Shortcode.Value,
            category.ParentCategoryId);
    }
}