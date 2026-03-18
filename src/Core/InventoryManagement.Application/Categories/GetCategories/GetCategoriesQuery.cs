using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Categories;
using InventoryManagement.Application.DTOs.Common;

namespace InventoryManagement.Application.Categories.GetCategories;

public sealed record GetCategoriesQuery(
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PagedResponse<CategoryResponseDto>>;