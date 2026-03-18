using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Common;
using InventoryManagement.Application.DTOs.Suppliers;

namespace InventoryManagement.Application.Suppliers.GetSuppliers;

public sealed record GetSuppliersQuery(
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PagedResponse<SupplierResponseDto>>;