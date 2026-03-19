using InventoryManagement.Domain.Abstractions;

namespace InventoryManagement.Domain.Ports.Gateways.Wms;

public interface IWmsGateway
{
    Task<Result<WmsCreateProductResponseDto>> CreateProductAsync(
        WmsCreateProductRequestDto request,
        CancellationToken cancellationToken = default);

    Task<Result> DispatchProductAsync(
        Guid productId,
        CancellationToken cancellationToken = default);
}