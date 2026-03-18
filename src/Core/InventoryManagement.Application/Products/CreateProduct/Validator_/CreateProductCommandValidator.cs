using FluentValidation;

namespace InventoryManagement.Application.Products.CreateProduct;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.SupplierId)
            .NotEmpty();

        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.AcquisitionCost)
            .GreaterThan(0);

        RuleFor(x => x.AcquisitionCostUsd)
            .GreaterThan(0);

        RuleFor(x => x.AcquireDate)
            .NotEmpty();
    }
}