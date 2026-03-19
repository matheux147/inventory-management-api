using InventoryManagement.Application.Abstractions.Errors;
using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Categories;
using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Ports.Context;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Application.Categories.CreateCategory;

public sealed class CreateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCategoryCommand, CategoryResponseDto>
{
    public async Task<Result<CategoryResponseDto>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var shortcode = CategoryShortcode.Create(request.Shortcode);

            var shortcodeAlreadyExists = await categoryRepository.ExistsByShortcodeAsync(
                shortcode,
                cancellationToken);

            if (shortcodeAlreadyExists)
                return Result<CategoryResponseDto>.Failure(ApplicationErrors.CategoryShortcodeAlreadyExists);

            Category? parentCategory = null;

            if (request.ParentCategoryId.HasValue)
            {
                parentCategory = await categoryRepository.GetByIdAsync(
                    request.ParentCategoryId.Value,
                    cancellationToken);

                if (parentCategory is null)
                    return Result<CategoryResponseDto>.Failure(ApplicationErrors.CategoryNotFound);
            }

            var category = Category.Create(
                request.Name,
                shortcode,
                parentCategory);

            await categoryRepository.AddAsync(category, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<CategoryResponseDto>.Success(category.Map());
        }
        catch (DomainException exception)
        {
            return Result<CategoryResponseDto>.Failure(new Error(exception.Message));
        }
    }
}