using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Ports.Repositories.Common;

namespace InventoryManagement.Domain.Ports.Repositories;

public interface IReadRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<TEntity>> GetAllAsync(
        PaginationParams paginationParams,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}