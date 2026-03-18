using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Common;
using InventoryManagement.Application.DTOs.Suppliers;
using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.Ports.Repositories.Common;

namespace InventoryManagement.Application.Suppliers.GetSuppliers;

public sealed class GetSuppliersQueryHandler(
    ISupplierRepository supplierRepository)
    : IQueryHandler<GetSuppliersQuery, PagedResponse<SupplierResponseDto>>
{
    public async Task<Result<PagedResponse<SupplierResponseDto>>> Handle(
        GetSuppliersQuery request,
        CancellationToken cancellationToken)
    {
        var pagedResult = await supplierRepository.GetAllAsync(
            new PaginationParams(request.PageNumber, request.PageSize),
            cancellationToken);

        return Result<PagedResponse<SupplierResponseDto>>.Success(pagedResult.Map());
    }
}