namespace InventoryManagement.Domain.Ports.Gateways.Email;

public interface IEmailGateway
{
    Task SendAsync(
        EmailSendRequestDto request,
        CancellationToken cancellationToken = default);
}
