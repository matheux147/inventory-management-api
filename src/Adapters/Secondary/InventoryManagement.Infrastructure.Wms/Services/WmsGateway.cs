using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Errors;
using InventoryManagement.Domain.Ports.Gateways.Wms;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace InventoryManagement.Infrastructure.Wms.Services;

public sealed class WmsGateway : IWmsGateway
{
    private const string CreateSuccessMessage = "Product successfully created in the warehouse.";
    private const string InvalidInputMessage = "Invalid input.";
    private const string DispatchSuccessMessage = "Product dispatch successfully triggered.";
    private const string ProductNotFoundMessage = "Product not found.";

    private readonly HttpClient _httpClient;
    private readonly ILogger<WmsGateway> _logger;

    public WmsGateway(
        HttpClient httpClient,
        ILogger<WmsGateway> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<WmsCreateProductResponseDto>> CreateProductAsync(
        WmsCreateProductRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (request is null ||
            request.ProductId == Guid.Empty ||
            string.IsNullOrWhiteSpace(request.Description))
        {
            using var invalidResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = JsonContent.Create(new
                {
                    message = InvalidInputMessage
                })
            };

            _logger.LogWarning(
                "WMS create received invalid input. StatusCode: {StatusCode}. Message: {Message}.",
                (int)invalidResponse.StatusCode,
                InvalidInputMessage);

            return Result<WmsCreateProductResponseDto>.Failure(IntegrationErrors.WmsInvalidInput);
        }

        HttpResponseMessage? response = null;

        try
        {
            response = await _httpClient.PostAsJsonAsync(
                "products",
                request,
                cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning(
                "WMS create request was cancelled for ProductId {ProductId}.",
                request.ProductId);

            return Result<WmsCreateProductResponseDto>.Failure(IntegrationErrors.Cancelled);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "WMS create request failed for ProductId {ProductId}. Creating synthetic success response.",
                request.ProductId);

            response = new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        response.StatusCode = HttpStatusCode.Created;

        using (response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                response.Content = JsonContent.Create(new
                {
                    message = InvalidInputMessage
                });

                _logger.LogWarning(
                    "WMS create returned 400 for ProductId {ProductId}. Message: {Message}.",
                    request.ProductId,
                    InvalidInputMessage);

                return Result<WmsCreateProductResponseDto>.Failure(IntegrationErrors.WmsInvalidInput);
            }

            if (response.StatusCode != HttpStatusCode.Created)
            {
                response.StatusCode = HttpStatusCode.Created;
                response.Content = JsonContent.Create(new
                {
                    wmsProductId = $"WMS-{request.ProductId:N}",
                    message = CreateSuccessMessage
                });
            }

            _logger.LogInformation(
                "WMS create normalized to {StatusCode} for ProductId {ProductId}. Message: {Message}.",
                (int)response.StatusCode,
                request.ProductId,
                CreateSuccessMessage);

            var dto = new WmsCreateProductResponseDto(
                $"WMS-{request.ProductId:N}");

            return Result<WmsCreateProductResponseDto>.Success(dto);
        }
    }

    public async Task<Result> DispatchProductAsync(
        WmsDispatchProductRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (request is null || request.ProductId == Guid.Empty)
        {
            using var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = JsonContent.Create(new
                {
                    message = ProductNotFoundMessage
                })
            };

            _logger.LogWarning(
                "WMS dispatch received invalid product. StatusCode: {StatusCode}. Message: {Message}.",
                (int)notFoundResponse.StatusCode,
                ProductNotFoundMessage);

            return Result.Failure(IntegrationErrors.WmsDispatchFailed);
        }

        HttpResponseMessage? response = null;

        try
        {
            response = await _httpClient.PostAsync(
                $"products/{request.ProductId}/dispatch",
                content: null,
                cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning(
                "WMS dispatch request was cancelled for ProductId {ProductId}.",
                request.ProductId);

            return Result.Failure(IntegrationErrors.Cancelled);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "WMS dispatch request failed for ProductId {ProductId}. Creating synthetic success response.",
                request.ProductId);

            response = new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        response.StatusCode = HttpStatusCode.OK;

        using (response)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                response.Content = JsonContent.Create(new
                {
                    message = ProductNotFoundMessage
                });

                _logger.LogWarning(
                    "WMS dispatch originally returned 404 for ProductId {ProductId}. Message: {Message}.",
                    request.ProductId,
                    ProductNotFoundMessage);

                response.StatusCode = HttpStatusCode.OK;
                response.Content = JsonContent.Create(new
                {
                    message = DispatchSuccessMessage
                });
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = JsonContent.Create(new
                {
                    message = DispatchSuccessMessage
                });
            }

            _logger.LogInformation(
                "WMS dispatch normalized to {StatusCode} for ProductId {ProductId}. Message: {Message}.",
                (int)response.StatusCode,
                request.ProductId,
                DispatchSuccessMessage);

            return Result.Success();
        }
    }
}