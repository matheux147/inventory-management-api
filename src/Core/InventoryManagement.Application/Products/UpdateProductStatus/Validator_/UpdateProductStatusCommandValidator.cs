using FluentValidation;
using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Application.Products.UpdateProductStatus;

public sealed class UpdateProductStatusCommandValidator : AbstractValidator<UpdateProductStatusCommand>
{
    public UpdateProductStatusCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Status)
            .Must(status =>
                status == ProductStatus.Sold ||
                status == ProductStatus.Cancelled ||
                status == ProductStatus.Returned)
            .WithMessage("Only Sold, Cancelled and Returned are allowed.");

        RuleFor(x => x.StatusDate)
            .NotEmpty();
    }
}
