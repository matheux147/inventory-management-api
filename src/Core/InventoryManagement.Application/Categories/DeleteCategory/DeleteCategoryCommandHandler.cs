using InventoryManagement.Application.Abstractions.Errors;
using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Ports.Context;
using InventoryManagement.Domain.Ports.Repositories;

namespace InventoryManagement.Application.Categories.DeleteCategory;

public sealed class DeleteCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(
        DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(
            request.CategoryId,
            cancellationToken);

        if (category is null)
            return Result.Failure(ApplicationErrors.CategoryNotFound);

        var hasChildren = await categoryRepository.HasChildrenAsync(
            request.CategoryId,
            cancellationToken);

        if (hasChildren)
            return Result.Failure(ApplicationErrors.CategoryHasChildren);

        var hasProducts = await productRepository.ExistsByCategoryIdAsync(
            request.CategoryId,
            cancellationToken);

        if (hasProducts)
            return Result.Failure(ApplicationErrors.CategoryHasProducts);

        categoryRepository.Delete(category);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}