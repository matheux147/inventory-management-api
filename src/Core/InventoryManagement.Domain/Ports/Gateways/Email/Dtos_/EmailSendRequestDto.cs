namespace InventoryManagement.Domain.Ports.Gateways.Email;

public sealed record EmailSendRequestDto(
    string To,
    string Subject,
    string Body);
