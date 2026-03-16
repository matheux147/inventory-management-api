namespace InventoryManagement.Domain.Ports.Gateways.Audit;

public interface IAuditLogGateway
{
    Task CreateEntryAsync(
        AuditLogCreateEntryRequestDto request,
        CancellationToken cancellationToken = default);
}
