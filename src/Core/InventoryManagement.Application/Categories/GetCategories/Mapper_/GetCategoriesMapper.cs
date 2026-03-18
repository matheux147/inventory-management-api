using InventoryManagement.Application.DTOs.Categories;
using InventoryManagement.Application.DTOs.Common;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Ports.Repositories.Common;

namespace InventoryManagement.Application.Categories.GetCategories;

public static class GetCategoriesMapper
{
    public static CategoryResponseDto Map(this Category category)
    {
        return new CategoryResponseDto(
            category.Id,
            category.Name,
            category.Shortcode.Value,
            category.ParentCategoryId);
    }

    public static PagedResponse<CategoryResponseDto> Map(this PagedResult<Category> pagedResult)
    {
        var items = pagedResult.Items
            .Select(x => x.Map())
            .ToList();

        return new PagedResponse<CategoryResponseDto>(
            items,
            pagedResult.TotalCount,
            pagedResult.PageNumber,
            pagedResult.PageSize,
            pagedResult.TotalPages);
    }
}