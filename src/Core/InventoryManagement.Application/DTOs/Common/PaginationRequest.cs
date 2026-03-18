namespace InventoryManagement.Application.DTOs.Common;

public sealed record PaginationRequest(
    int PageNumber = 1,
    int PageSize = 10);