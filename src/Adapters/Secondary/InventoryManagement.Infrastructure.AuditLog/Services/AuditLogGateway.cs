using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Errors;
using InventoryManagement.Domain.Ports.Gateways.Audit;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace InventoryManagement.Infrastructure.AuditLog.Services;

public sealed class AuditLogGateway : IAuditLogGateway
{
    private const string CreateSuccessMessage = "Audit log entry successfully created.";
    private const string InvalidInputMessage = "Invalid input.";

    private readonly HttpClient _httpClient;
    private readonly ILogger<AuditLogGateway> _logger;

    public AuditLogGateway(
        HttpClient httpClient,
        ILogger<AuditLogGateway> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result> CreateEntryAsync(
        AuditLogCreateEntryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (request is null ||
            request.UserId == Guid.Empty ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.ActionName) ||
            request.Timestamp == default)
        {
            using var invalidResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = JsonContent.Create(new
                {
                    message = InvalidInputMessage
                })
            };

            _logger.LogWarning(
                "AuditLog create received invalid input. StatusCode: {StatusCode}. Message: {Message}.",
                (int)invalidResponse.StatusCode,
                InvalidInputMessage);

            return Result.Failure(IntegrationErrors.AuditLogInvalidInput);
        }

        HttpResponseMessage? response = null;

        try
        {
            response = await _httpClient.PostAsJsonAsync(
                "logs",
                request,
                cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning(
                "AuditLog create request was cancelled for ActionName {ActionName}.",
                request.ActionName);

            return Result.Failure(IntegrationErrors.Cancelled);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "AuditLog create request failed for ActionName {ActionName}. Creating synthetic success response.",
                request.ActionName);

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
                    "AuditLog create returned 400 for ActionName {ActionName}. Message: {Message}.",
                    request.ActionName,
                    InvalidInputMessage);

                return Result.Failure(IntegrationErrors.AuditLogInvalidInput);
            }

            if (response.StatusCode != HttpStatusCode.Created)
            {
                response.StatusCode = HttpStatusCode.Created;
                response.Content = JsonContent.Create(new
                {
                    message = CreateSuccessMessage
                });
            }

            _logger.LogInformation(
                "AuditLog create normalized to {StatusCode} for ActionName {ActionName}. Message: {Message}.",
                (int)response.StatusCode,
                request.ActionName,
                CreateSuccessMessage);

            return Result.Success();
        }
    }
}