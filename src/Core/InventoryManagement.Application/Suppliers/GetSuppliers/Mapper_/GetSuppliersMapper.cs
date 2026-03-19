using InventoryManagement.Application.DTOs.Common;
using InventoryManagement.Application.DTOs.Suppliers;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Ports.Repositories.Common;

namespace InventoryManagement.Application.Suppliers.GetSuppliers;

public static class GetSuppliersMapper
{
    public static SupplierResponseDto Map(this Supplier supplier)
    {
        return new SupplierResponseDto(
            supplier.Id,
            supplier.Name,
            supplier.Email.Value,
            supplier.Currency.Value,
            supplier.Country.Value);
    }

    public static PagedResponse<SupplierResponseDto> Map(this PagedResult<Supplier> pagedResult)
    {
        var items = pagedResult.Items
            .Select(x => x.Map())
            .ToList();

        return new PagedResponse<SupplierResponseDto>(
            items,
            pagedResult.TotalCount,
            pagedResult.PageNumber,
            pagedResult.PageSize,
            pagedResult.TotalPages);
    }
}