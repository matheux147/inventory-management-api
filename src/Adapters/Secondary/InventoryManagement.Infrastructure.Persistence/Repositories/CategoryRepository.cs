using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.ValueObjects;
using InventoryManagement.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Persistence.Repositories;

public sealed class CategoryRepository(InventoryManagementDbContext context) : DeletableRepository<Category>(context), ICategoryRepository
{
    public async Task<Category?> GetByShortcodeAsync(
        CategoryShortcode shortcode,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Shortcode == shortcode, cancellationToken);
    }

    public async Task<bool> ExistsByShortcodeAsync(
        CategoryShortcode shortcode,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .AsNoTracking()
            .AnyAsync(x => x.Shortcode == shortcode, cancellationToken);
    }

    public async Task<bool> HasChildrenAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .AsNoTracking()
            .AnyAsync(x => x.ParentCategoryId == categoryId, cancellationToken);
    }
}