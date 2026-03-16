namespace InventoryManagement.Domain.Ports.Gateways.Audit;

public sealed record AuditLogCreateEntryRequestDto(
    Guid UserId,
    string Email,
    string ActionName,
    DateTime Timestamp);
