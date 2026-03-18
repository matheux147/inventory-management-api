using InventoryManagement.Application.DTOs.Products;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Products.CreateProduct;

public static class CreateProductMapper
{
    public static ProductResponseDto Map(this Product product)
    {
        return new ProductResponseDto(
            product.Id,
            product.SupplierId,
            product.CategoryId,
            product.Description,
            product.AcquisitionCost,
            product.AcquisitionCostUsd,
            product.AcquireDate,
            product.SoldDate,
            product.CancelDate,
            product.ReturnDate,
            product.Status.ToString());
    }
}