namespace InventoryManagement.Domain.Ports.Context;

public interface ICurrentUserContext
{
    Guid UserId { get; }
    string Email { get; }
}