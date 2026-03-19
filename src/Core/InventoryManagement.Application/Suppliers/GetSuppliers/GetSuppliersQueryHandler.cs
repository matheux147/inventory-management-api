using InventoryManagement.Application.Abstractions.Caching;
using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Common;
using InventoryManagement.Application.DTOs.Suppliers;
using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Ports.Cache;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.Ports.Repositories.Common;

namespace InventoryManagement.Application.Suppliers.GetSuppliers;

public sealed class GetSuppliersQueryHandler(
    ISupplierRepository supplierRepository,
    IAppCache appCache)
    : IQueryHandler<GetSuppliersQuery, PagedResponse<SupplierResponseDto>>
{
    public async Task<Result<PagedResponse<SupplierResponseDto>>> Handle(
        GetSuppliersQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.SuppliersList(request.PageNumber, request.PageSize);

        var response = await appCache.GetOrCreateAsync(
            cacheKey,
            async cancel =>
            {
                var pagedResult = await supplierRepository.GetAllAsync(
                    new PaginationParams(request.PageNumber, request.PageSize),
                    cancel);

                return pagedResult.Map();
            },
            tags: [CacheTags.Suppliers],
            cancellationToken: cancellationToken);

        return Result<PagedResponse<SupplierResponseDto>>.Success(response);
    }
}