using InventoryManagement.Application.Abstractions.Errors;
using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Products;
using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Errors;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Ports.Context;
using InventoryManagement.Domain.Ports.Gateways.Audit;
using InventoryManagement.Domain.Ports.Gateways.Email;
using InventoryManagement.Domain.Ports.Gateways.Wms;
using InventoryManagement.Domain.Ports.Repositories;

namespace InventoryManagement.Application.Products.UpdateProductStatus;

public sealed class ChangeProductStatusCommandHandler(
    IProductRepository productRepository,
    IEmailGateway emailGateway,
    IWmsGateway wmsGateway,
    IAuditLogGateway auditLogGateway,
    ICurrentUserContext userContext,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateProductStatusCommand, UpdateProductStatusResponseDto>
{
    public async Task<Result<UpdateProductStatusResponseDto>> Handle(
        UpdateProductStatusCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var product = await productRepository.GetByIdWithDetailsAsync(
                request.ProductId,
                cancellationToken);

            if (product is null)
                return Result<UpdateProductStatusResponseDto>.Failure(ApplicationErrors.ProductNotFound);

            switch (request.Status)
            {
                case ProductStatus.Sold:
                    product.Sell(request.StatusDate);
                    break;

                case ProductStatus.Cancelled:
                    product.Cancel(request.StatusDate);
                    break;

                case ProductStatus.Returned:
                    product.Return(request.StatusDate);
                    break;
            }

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            if (request.Status == ProductStatus.Sold)
            {
                var sendEmailResult = await emailGateway.SendAsync(
                    new EmailSendRequestDto(
                        product.Supplier.Email.Value,
                        "Product sold",
                        $"The product {product.Id} has been sold."),
                    cancellationToken);

                if (sendEmailResult.IsFailure)
                    return Result<UpdateProductStatusResponseDto>.Failure(sendEmailResult.Error ?? IntegrationErrors.EmailSendFailed);

                var dispatchResult = await wmsGateway.DispatchProductAsync(
                    product.Id,
                    cancellationToken);

                if (dispatchResult.IsFailure)
                    return Result<UpdateProductStatusResponseDto>.Failure(dispatchResult.Error ?? IntegrationErrors.WmsDispatchFailed);
            }

            var auditLogResult = await auditLogGateway.CreateEntryAsync(
                new AuditLogCreateEntryRequestDto(
                    userContext.UserId,
                    userContext.Email,
                    "PRODUCT_STATUS_CHANGED",
                    dateTimeProvider.UtcNow),
                cancellationToken);

            if (auditLogResult.IsFailure)
                return Result<UpdateProductStatusResponseDto>.Failure(auditLogResult.Error ?? IntegrationErrors.AuditLogCreateFailed);

            return Result<UpdateProductStatusResponseDto>.Success(product.Map());
        }
        catch (DomainException exception)
        {
            return Result<UpdateProductStatusResponseDto>.Failure(new Error(exception.Message));
        }
    }
}