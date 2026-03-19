namespace InventoryManagement.Api.Controllers.Categories;

public sealed record CreateCategoryRequestDto(
    string Name,
    string Shortcode,
    Guid? ParentCategoryId);