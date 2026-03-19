using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Categories;
using InventoryManagement.Application.DTOs.Common;
using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.Ports.Repositories.Common;

namespace InventoryManagement.Application.Categories.GetCategories;

public sealed class GetCategoriesQueryHandler(
    ICategoryRepository categoryRepository)
    : IQueryHandler<GetCategoriesQuery, PagedResponse<CategoryResponseDto>>
{
    public async Task<Result<PagedResponse<CategoryResponseDto>>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var pagedResult = await categoryRepository.GetAllAsync(
            new PaginationParams(request.PageNumber, request.PageSize),
            cancellationToken);

        return Result<PagedResponse<CategoryResponseDto>>.Success(pagedResult.Map());
    }
}