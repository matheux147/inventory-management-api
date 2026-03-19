using InventoryManagement.Application.DTOs.Products;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Products.UpdateProductStatus;

public static class ChangeProductStatusMapper
{
    public static UpdateProductStatusResponseDto Map(this Product product)
    {
        return new UpdateProductStatusResponseDto(
            product.Id,
            product.Status.ToString(),
            product.SoldDate,
            product.CancelDate,
            product.ReturnDate);
    }
}