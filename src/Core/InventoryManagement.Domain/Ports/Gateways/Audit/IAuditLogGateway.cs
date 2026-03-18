using InventoryManagement.Domain.Abstractions;

namespace InventoryManagement.Domain.Ports.Gateways.Audit;

public interface IAuditLogGateway
{
    Task<Result> CreateEntryAsync(
        AuditLogCreateEntryRequestDto request,
        CancellationToken cancellationToken = default);
}