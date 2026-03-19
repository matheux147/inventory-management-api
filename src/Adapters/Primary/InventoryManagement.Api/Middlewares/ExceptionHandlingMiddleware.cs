using System.Globalization;
using System.Reflection;
using System.Text.Json;
using FluentValidation;
using InventoryManagement.Api.Resources;
using InventoryManagement.Domain.Exceptions;

namespace InventoryManagement.Api.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors
                .Select(error => new
                {
                    property = error.PropertyName,
                    message = error.ErrorMessage
                })
                .ToList();

            var response = new
            {
                code = "validation.failed",
                message = "Validation failed.",
                errors
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (DomainException ex)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            context.Response.ContentType = "application/json";

            var response = new
            {
                code = ex.Code,
                message = Localize(ex.Code)
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            var code = ExtractCode(ex);

            var response = new
            {
                code = code ?? "internal.server_error",
                message = code is not null
                    ? Localize(code)
                    : "Internal server error."
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    private static string Localize(string code)
    {
        var localized = Messages.ResourceManager.GetString(
            code,
            CultureInfo.CurrentUICulture);

        return string.IsNullOrWhiteSpace(localized)
            ? code
            : localized;
    }

    private static string? ExtractCode(Exception ex)
    {
        var codeProperty = ex.GetType().GetProperty("Code", BindingFlags.Public | BindingFlags.Instance);
        if (codeProperty?.PropertyType == typeof(string))
            return codeProperty.GetValue(ex) as string;

        var errorProperty = ex.GetType().GetProperty("Error", BindingFlags.Public | BindingFlags.Instance);
        if (errorProperty is not null)
        {
            var errorObject = errorProperty.GetValue(ex);
            if (errorObject is not null)
            {
                var nestedCodeProperty = errorObject.GetType().GetProperty("Code", BindingFlags.Public | BindingFlags.Instance);
                if (nestedCodeProperty?.PropertyType == typeof(string))
                    return nestedCodeProperty.GetValue(errorObject) as string;
            }
        }

        return null;
    }
}