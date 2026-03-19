using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Api.Controllers.Products;

public sealed record UpdateProductStatusRequestDto(
    ProductStatus Status,
    DateTime StatusDate);
