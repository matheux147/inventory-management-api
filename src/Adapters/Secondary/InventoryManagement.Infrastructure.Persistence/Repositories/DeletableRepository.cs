using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Infrastructure.Persistence.DbContexts;

namespace InventoryManagement.Infrastructure.Persistence.Repositories;

public abstract class DeletableRepository<TEntity>(InventoryManagementDbContext context) : BaseRepository<TEntity>(context), IDeletableRepository<TEntity>
    where TEntity : Entity
{
    public virtual void Delete(TEntity entity)
    {
        Set.Remove(entity);
    }
}