using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Ports.Repositories;

public interface IDeletableRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    void Delete(TEntity entity);
}