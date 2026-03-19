using InventoryManagement.Domain.Ports.Context;

namespace InventoryManagement.Infrastructure.Shared.Services;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}