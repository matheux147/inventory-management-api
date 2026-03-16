using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Ports.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByIdWithDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCategoryIdAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Product>> GetBySupplierIdAsync(
        Guid supplierId,
        CancellationToken cancellationToken = default);
}