namespace InventoryManagement.Api.Controllers.Suppliers;

public sealed record CreateSupplierRequestDto(
    string Name,
    string Email,
    string Currency,
    string Country);
