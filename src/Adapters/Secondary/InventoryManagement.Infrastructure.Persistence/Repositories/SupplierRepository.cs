using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.ValueObjects;
using InventoryManagement.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Infrastructure.Persistence.Repositories;

public sealed class SupplierRepository(InventoryManagementDbContext context) : BaseRepository<Supplier>(context), ISupplierRepository
{
    public async Task<Supplier?> GetByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .AsNoTracking()
            .AnyAsync(x => x.Email == email, cancellationToken);
    }
}