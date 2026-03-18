namespace InventoryManagement.Application.DTOs.Products;

public sealed record UpdateProductStatusResponseDto(
    Guid ProductId,
    string Status,
    DateTime? SoldDate,
    DateTime? CancelDate,
    DateTime? ReturnDate);