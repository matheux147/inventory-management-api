using InventoryManagement.Application.Abstractions.Caching;
using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Categories;
using InventoryManagement.Application.DTOs.Common;
using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Ports.Cache;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.Ports.Repositories.Common;

namespace InventoryManagement.Application.Categories.GetCategories;

public sealed class GetCategoriesQueryHandler(
    ICategoryRepository categoryRepository,
    IAppCache appCache)
    : IQueryHandler<GetCategoriesQuery, PagedResponse<CategoryResponseDto>>
{
    public async Task<Result<PagedResponse<CategoryResponseDto>>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.CategoriesList(request.PageNumber, request.PageSize);

        var response = await appCache.GetOrCreateAsync(
            cacheKey,
            async cancel =>
            {
                var pagedResult = await categoryRepository.GetAllAsync(
                    new PaginationParams(request.PageNumber, request.PageSize),
                    cancel);

                return pagedResult.Map();
            },
            tags: [CacheTags.Categories],
            cancellationToken: cancellationToken);

        return Result<PagedResponse<CategoryResponseDto>>.Success(response);
    }
}