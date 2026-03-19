using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Ports.Repositories;

public interface IRepository<TEntity> : IReadRepository<TEntity> where TEntity : Entity
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
}