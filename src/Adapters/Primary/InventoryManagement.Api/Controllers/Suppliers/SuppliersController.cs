using InventoryManagement.Api.Controllers.Common;
using InventoryManagement.Api.Controllers.Common.Extensions;
using InventoryManagement.Api.Resources;
using InventoryManagement.Application.DTOs.Common;
using InventoryManagement.Application.DTOs.Suppliers;
using InventoryManagement.Application.Suppliers.CreateSupplier;
using InventoryManagement.Application.Suppliers.GetSuppliers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace InventoryManagement.Api.Controllers.Suppliers;

[ApiController]
[Route("api/suppliers")]
public sealed class SuppliersController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(SupplierResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(
        [FromBody] CreateSupplierRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateSupplierCommand(
            request.Name,
            request.Email,
            request.Currency,
            request.Country);

        var result = await sender.Send(command, cancellationToken);

        return result.ToActionResult(
            this,
            value => StatusCode(StatusCodes.Status201Created, value));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<SupplierResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetSuppliersRequestDto request,
        CancellationToken cancellationToken)
    {
        var query = new GetSuppliersQuery(
            request.PageNumber,
            request.PageSize);

        var result = await sender.Send(query, cancellationToken);

        return result.ToActionResult(this, Ok);
    }
}