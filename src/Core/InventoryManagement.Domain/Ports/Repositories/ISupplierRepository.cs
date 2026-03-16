using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Ports.Repositories;

public interface ISupplierRepository : IRepository<Supplier>
{
    Task<Supplier?> GetByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default);
}