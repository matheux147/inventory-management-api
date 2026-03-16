namespace InventoryManagement.Domain.Ports.Context;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}