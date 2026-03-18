using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository(InventoryManagementDbContext context) : BaseRepository<Product>(context), IProductRepository
{
    public async Task<Product?> GetByIdWithDetailsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Include(x => x.Supplier)
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByCategoryIdAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .AsNoTracking()
            .AnyAsync(x => x.CategoryId == categoryId, cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetBySupplierIdAsync(
        Guid supplierId,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .AsNoTracking()
            .Where(x => x.SupplierId == supplierId)
            .Include(x => x.Category)
            .Include(x => x.Supplier)
            .ToListAsync(cancellationToken);
    }
}