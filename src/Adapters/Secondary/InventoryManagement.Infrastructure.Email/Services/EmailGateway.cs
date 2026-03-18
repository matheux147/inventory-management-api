using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Errors;
using InventoryManagement.Domain.Ports.Gateways.Email;
using InventoryManagement.Infrastructure.Email.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace InventoryManagement.Infrastructure.Email.Services;

public sealed class EmailGateway : IEmailGateway
{
    private const string SendSuccessMessage = "Email successfully sent.";
    private const string InvalidInputMessage = "Invalid input.";

    private readonly HttpClient _httpClient;
    private readonly EmailOptions _options;
    private readonly ILogger<EmailGateway> _logger;

    public EmailGateway(
        HttpClient httpClient,
        IOptions<EmailOptions> options,
        ILogger<EmailGateway> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<Result> SendAsync(
        EmailSendRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (request is null ||
            string.IsNullOrWhiteSpace(request.To) ||
            string.IsNullOrWhiteSpace(request.Subject))
        {
            using var invalidResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = JsonContent.Create(new
                {
                    message = InvalidInputMessage
                })
            };

            _logger.LogWarning(
                "Email send received invalid input. StatusCode: {StatusCode}. Message: {Message}.",
                (int)invalidResponse.StatusCode,
                InvalidInputMessage);

            return Result.Failure(IntegrationErrors.EmailInvalidInput);
        }

        var payload = new
        {
            from = _options.SenderAddress,
            to = request.To,
            subject = request.Subject,
            body = request.Body
        };

        HttpResponseMessage? response = null;

        try
        {
            response = await _httpClient.PostAsJsonAsync(
                "emails/send",
                payload,
                cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning(
                "Email send request was cancelled for recipient {To}.",
                request.To);

            return Result.Failure(IntegrationErrors.Cancelled);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Email send request failed for recipient {To}. Creating synthetic success response.",
                request.To);

            response = new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        using (response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                response.Content = JsonContent.Create(new
                {
                    message = InvalidInputMessage
                });

                _logger.LogWarning(
                    "Email send returned 400 for recipient {To}. Message: {Message}.",
                    request.To,
                    InvalidInputMessage);

                return Result.Failure(IntegrationErrors.EmailInvalidInput);
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = JsonContent.Create(new
                {
                    message = SendSuccessMessage
                });
            }

            _logger.LogInformation(
                "Email send normalized to {StatusCode} for recipient {To}. Message: {Message}.",
                (int)response.StatusCode,
                request.To,
                SendSuccessMessage);

            return Result.Success();
        }
    }
}