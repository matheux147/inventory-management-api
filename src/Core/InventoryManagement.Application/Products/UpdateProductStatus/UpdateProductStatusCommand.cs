using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Products;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Application.Products.UpdateProductStatus;

public sealed record UpdateProductStatusCommand(
    Guid ProductId,
    ProductStatus Status,
    DateTime StatusDate) : ICommand<UpdateProductStatusResponseDto>;