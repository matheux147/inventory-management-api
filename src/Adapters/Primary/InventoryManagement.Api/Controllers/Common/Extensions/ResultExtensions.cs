using System.Globalization;
using InventoryManagement.Api.Resources;
using InventoryManagement.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers.Common.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(
        this Result result,
        ControllerBase controller)
    {
        if (result.IsSuccess)
            return controller.NoContent();

        var response = BuildResponse(result.Error);

        return MapError(result.Error, response, controller);
    }

    public static IActionResult ToActionResult<T>(
        this Result<T> result,
        ControllerBase controller,
        Func<T, IActionResult> onSuccess)
    {
        if (result.IsSuccess)
            return onSuccess(result.Value!);

        var response = BuildResponse(result.Error);

        return MapError(result.Error, response, controller);
    }

    private static ErrorResponse BuildResponse(Error error)
    {
        var message = Messages.ResourceManager.GetString(
            error.Code,
            CultureInfo.CurrentUICulture);

        return new ErrorResponse(
            error.Code,
            string.IsNullOrWhiteSpace(message) ? error.Code : message);
    }

    private static ObjectResult MapError(
        Error error,
        ErrorResponse response,
        ControllerBase controller)
    {
        return error.Type switch
        {
            ErrorType.Validation => controller.BadRequest(response),
            ErrorType.NotFound => controller.NotFound(response),
            ErrorType.Conflict => controller.Conflict(response),
            ErrorType.Unauthorized => controller.Unauthorized(response),
            ErrorType.Forbidden => controller.StatusCode(StatusCodes.Status403Forbidden, response),
            _ => controller.UnprocessableEntity(response)
        };
    }
}