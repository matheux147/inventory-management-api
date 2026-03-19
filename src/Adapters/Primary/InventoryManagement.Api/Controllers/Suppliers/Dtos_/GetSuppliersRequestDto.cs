namespace InventoryManagement.Api.Controllers.Suppliers;

public sealed record GetSuppliersRequestDto(
    int PageNumber = 1,
    int PageSize = 10);
