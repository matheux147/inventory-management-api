namespace InventoryManagement.Api.Controllers.Categories;

public sealed record GetCategoriesRequestDto(
    int PageNumber = 1,
    int PageSize = 10);