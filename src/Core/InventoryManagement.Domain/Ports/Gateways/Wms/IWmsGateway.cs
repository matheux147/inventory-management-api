namespace InventoryManagement.Domain.Ports.Gateways.Wms;

public interface IWmsGateway
{
    Task<WmsCreateProductResponseDto> CreateProductAsync(
        WmsCreateProductRequestDto request,
        CancellationToken cancellationToken = default);

    Task DispatchProductAsync(
        WmsDispatchProductRequestDto request,
        CancellationToken cancellationToken = default);
}
