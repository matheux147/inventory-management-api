namespace InventoryManagement.Api.Controllers.Common;

public sealed record ErrorResponse(
    string Code,
    string Message);