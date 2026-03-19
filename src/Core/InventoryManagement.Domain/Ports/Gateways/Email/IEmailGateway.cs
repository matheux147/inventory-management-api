using InventoryManagement.Domain.Abstractions;

namespace InventoryManagement.Domain.Ports.Gateways.Email;

public interface IEmailGateway
{
    Task<Result> SendAsync(
        EmailSendRequestDto request,
        CancellationToken cancellationToken = default);
}