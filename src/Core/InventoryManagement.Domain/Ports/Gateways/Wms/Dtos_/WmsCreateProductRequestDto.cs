namespace InventoryManagement.Domain.Ports.Gateways.Wms;

public sealed record WmsCreateProductRequestDto(
    Guid ProductId,
    string Description,
    string? CategoryShortcode,
    Guid? SupplierId);
