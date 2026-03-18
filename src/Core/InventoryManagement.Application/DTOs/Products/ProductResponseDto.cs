namespace InventoryManagement.Application.DTOs.Products;

public sealed record ProductResponseDto(
    Guid Id,
    Guid SupplierId,
    Guid CategoryId,
    string Description,
    decimal AcquisitionCostInSupplierCurrency,
    decimal AcquisitionCostInUsd,
    DateTime AcquireDate,
    DateTime? SoldDate,
    DateTime? CancelDate,
    DateTime? ReturnDate,
    string Status);