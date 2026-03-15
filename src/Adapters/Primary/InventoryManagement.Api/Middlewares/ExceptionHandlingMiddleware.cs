using System.Net;
using System.Text.Json;
using InventoryManagement.Api.Resources;
using InventoryManagement.Domain.Exceptions;
using Microsoft.Extensions.Localization;

namespace InventoryManagement.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IStringLocalizer<Messages> _localizer;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        IStringLocalizer<Messages> localizer)
    {
        _next = next;
        _localizer = localizer;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            context.Response.ContentType = "application/json";

            var localizedMessage = _localizer[ex.Code];
            var message = localizedMessage.ResourceNotFound
                ? ex.Message
                : localizedMessage.Value;

            var response = new
            {
                error = new
                {
                    code = ex.Code,
                    message
                }
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
