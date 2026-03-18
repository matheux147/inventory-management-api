using InventoryManagement.Application.DTOs.Suppliers;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Application.Suppliers.CreateSupplier;

public static class CreateSupplierMapper
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
}