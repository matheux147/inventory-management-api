using InventoryManagement.Application.Abstractions.Errors;
using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Products;
using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Errors;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Ports.Context;
using InventoryManagement.Domain.Ports.Gateways.Audit;
using InventoryManagement.Domain.Ports.Gateways.Wms;
using InventoryManagement.Domain.Ports.Repositories;

namespace InventoryManagement.Application.Products.CreateProduct;

public sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    ISupplierRepository supplierRepository,
    ICategoryRepository categoryRepository,
    IWmsGateway wmsGateway,
    IAuditLogGateway auditLogGateway,
    ICurrentUserContext userContext,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateProductCommand, ProductResponseDto>
{
    public async Task<Result<ProductResponseDto>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var supplier = await supplierRepository.GetByIdAsync(
                request.SupplierId,
                cancellationToken);

            if (supplier is null)
                return Result<ProductResponseDto>.Failure(ApplicationErrors.SupplierNotFound);

            var category = await categoryRepository.GetByIdAsync(
                request.CategoryId,
                cancellationToken);

            if (category is null)
                return Result<ProductResponseDto>.Failure(ApplicationErrors.CategoryNotFound);

            var product = Product.Create(
                supplier,
                category,
                request.Description,
                request.AcquisitionCost,
                request.AcquisitionCostUsd,
                request.AcquireDate);

            await productRepository.AddAsync(product, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var createWmsResult = await wmsGateway.CreateProductAsync(
                new WmsCreateProductRequestDto(
                    product.Id,
                    product.Description,
                    category.Shortcode.Value,
                    supplier.Id),
                cancellationToken);

            if (createWmsResult.IsFailure)
                return Result<ProductResponseDto>.Failure(createWmsResult.Error ?? IntegrationErrors.WmsCreateFailed);

            var createAuditLogResult = await auditLogGateway.CreateEntryAsync(
                new AuditLogCreateEntryRequestDto(
                    userContext.UserId,
                    userContext.Email,
                    "PRODUCT_CREATED",
                    dateTimeProvider.UtcNow),
                cancellationToken);

            if (createAuditLogResult.IsFailure)
                return Result<ProductResponseDto>.Failure(createAuditLogResult.Error ?? IntegrationErrors.AuditLogCreateFailed);

            return Result<ProductResponseDto>.Success(product.Map());
        }
        catch (DomainException exception)
        {
            return Result<ProductResponseDto>.Failure(new Error(exception.Message));
        }
    }
}