namespace InventoryManagement.Application.DTOs.Suppliers;

public sealed record SupplierResponseDto(
    Guid Id,
    string Name,
    string Email,
    string Currency,
    string Country);