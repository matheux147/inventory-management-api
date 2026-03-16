namespace InventoryManagement.Domain.Ports.Context;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}