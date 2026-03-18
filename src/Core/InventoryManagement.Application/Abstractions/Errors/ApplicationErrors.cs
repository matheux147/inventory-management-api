using InventoryManagement.Domain.Abstractions;

namespace InventoryManagement.Application.Abstractions.Errors;

public static class ApplicationErrors
{
    public static readonly Error CategoryNotFound =
        new("category.not_found", ErrorType.NotFound);

    public static readonly Error CategoryShortcodeAlreadyExists =
        new("category.shortcode_already_exists", ErrorType.Conflict);

    public static readonly Error CategoryHasChildren =
        new("category.has_children", ErrorType.Conflict);

    public static readonly Error CategoryHasProducts =
        new("category.has_products", ErrorType.Conflict);

    public static readonly Error SupplierNotFound =
        new("supplier.not_found", ErrorType.NotFound);

    public static readonly Error SupplierEmailAlreadyExists =
        new("supplier.email_already_exists", ErrorType.Conflict);

    public static readonly Error ProductNotFound =
        new("product.not_found", ErrorType.NotFound);
}