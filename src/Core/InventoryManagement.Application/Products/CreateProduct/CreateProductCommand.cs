using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Products;

namespace InventoryManagement.Application.Products.CreateProduct;

public sealed record CreateProductCommand(
    Guid SupplierId,
    Guid CategoryId,
    string Description,
    decimal AcquisitionCost,
    decimal AcquisitionCostUsd,
    DateTime AcquireDate) : ICommand<ProductResponseDto>;