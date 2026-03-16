using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Ports.Repositories;

public interface ICategoryRepository : IDeletableRepository<Category>
{
    Task<Category?> GetByShortcodeAsync(
        CategoryShortcode shortcode,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByShortcodeAsync(
        CategoryShortcode shortcode,
        CancellationToken cancellationToken = default);

    Task<bool> HasChildrenAsync(
        Guid categoryId,
        CancellationToken cancellationToken = default);
}