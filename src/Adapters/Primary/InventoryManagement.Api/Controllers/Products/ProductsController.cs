using InventoryManagement.Api.Controllers.Common;
using InventoryManagement.Api.Controllers.Common.Extensions;
using InventoryManagement.Api.Resources;
using InventoryManagement.Application.DTOs.Products;
using InventoryManagement.Application.Products.CreateProduct;
using InventoryManagement.Application.Products.UpdateProductStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace InventoryManagement.Api.Controllers.Products;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(
            request.SupplierId,
            request.CategoryId,
            request.Description,
            request.AcquisitionCost,
            request.AcquisitionCostUsd,
            request.AcquireDate);

        var result = await sender.Send(command, cancellationToken);

        return result.ToActionResult(
            this,
            value => StatusCode(StatusCodes.Status201Created, value));
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(UpdateProductStatusResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeStatus(
        [FromRoute] Guid id,
        [FromBody] UpdateProductStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProductStatusCommand(
            id,
            request.Status,
            request.StatusDate);

        var result = await sender.Send(command, cancellationToken);

        return result.ToActionResult(this, Ok);
    }
}