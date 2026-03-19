namespace InventoryManagement.Api.Controllers.Products;

public sealed record CreateProductRequestDto(
    Guid SupplierId,
    Guid CategoryId,
    string Description,
    decimal AcquisitionCost,
    decimal AcquisitionCostUsd,
    DateTime AcquireDate);
