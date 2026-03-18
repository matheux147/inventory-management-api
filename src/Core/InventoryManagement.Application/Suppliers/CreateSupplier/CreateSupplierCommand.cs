using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Suppliers;

namespace InventoryManagement.Application.Suppliers.CreateSupplier;

public sealed record CreateSupplierCommand(
    string Name,
    string Email,
    string Currency,
    string Country) : ICommand<SupplierResponseDto>;