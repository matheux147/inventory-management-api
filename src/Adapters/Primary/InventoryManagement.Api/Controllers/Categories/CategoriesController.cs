using InventoryManagement.Api.Controllers.Common;
using InventoryManagement.Api.Controllers.Common.Extensions;
using InventoryManagement.Api.Resources;
using InventoryManagement.Application.Categories.CreateCategory;
using InventoryManagement.Application.Categories.DeleteCategory;
using InventoryManagement.Application.Categories.GetCategories;
using InventoryManagement.Application.DTOs.Categories;
using InventoryManagement.Application.DTOs.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace InventoryManagement.Api.Controllers.Categories;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(
            request.Name,
            request.Shortcode,
            request.ParentCategoryId);

        var result = await sender.Send(command, cancellationToken);

        return result.ToActionResult(
            this,
            value => StatusCode(StatusCodes.Status201Created, value));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<CategoryResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetCategoriesRequestDto request,
        CancellationToken cancellationToken)
    {
        var query = new GetCategoriesQuery(
            request.PageNumber,
            request.PageSize);

        var result = await sender.Send(query, cancellationToken);

        return result.ToActionResult(this, Ok);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(id);

        var result = await sender.Send(command, cancellationToken);

        return result.ToActionResult(this);
    }
}