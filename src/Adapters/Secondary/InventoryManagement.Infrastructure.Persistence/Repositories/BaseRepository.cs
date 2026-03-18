using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.Ports.Repositories.Common;
using InventoryManagement.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository<TEntity>(InventoryManagementDbContext context) : IRepository<TEntity>
    where TEntity : Entity
{
    protected readonly InventoryManagementDbContext Context = context;
    protected readonly DbSet<TEntity> Set = context.Set<TEntity>();

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Set.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public virtual async Task<PagedResult<TEntity>> GetAllAsync(
        PaginationParams paginationParams,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await Set
            .AsNoTracking()
            .CountAsync(cancellationToken);

        var items = await Set
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip(paginationParams.Skip)
            .Take(paginationParams.NormalizedPageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<TEntity>(
            items,
            totalCount,
            paginationParams.NormalizedPageNumber,
            paginationParams.NormalizedPageSize);
    }

    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Set
            .AsNoTracking()
            .AnyAsync(x => x.Id == id, cancellationToken);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Set.AddAsync(entity, cancellationToken);
    }

    public virtual void Update(TEntity entity)
    {
        Set.Update(entity);
    }
}