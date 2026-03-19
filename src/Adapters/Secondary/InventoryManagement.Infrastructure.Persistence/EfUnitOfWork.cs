using InventoryManagement.Domain.Ports.Context;
using InventoryManagement.Infrastructure.Persistence.DbContexts;

namespace InventoryManagement.Infrastructure.Persistence;

public sealed class EfUnitOfWork(InventoryManagementDbContext context) : IUnitOfWork
{
    private readonly InventoryManagementDbContext _context = context;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}