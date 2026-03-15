namespace InventoryManagement.Domain.Constants;

public static class ProductMessages
{
    public const string CancelledProductCannotBeSold = "product.sell.cancelled";
    public const string ReturnedProductCannotBeSold = "product.sell.returned";
    public const string ProductAlreadySold = "product.sell.already_sold";
    public const string SoldDateCannotBeEarlierThanAcquireDate = "product.sell.invalid_date";

    public const string ProductAlreadyCancelled = "product.cancel.already_cancelled";
    public const string ReturnedProductCannotBeCancelled = "product.cancel.returned";
    public const string CancelDateCannotBeEarlierThanAcquireDate = "product.cancel.invalid_acquire_date";
    public const string CancelDateCannotBeEarlierThanSoldDate = "product.cancel.invalid_sold_date";

    public const string ProductAlreadyReturned = "product.return.already_returned";
    public const string CancelledProductCannotBeReturned = "product.return.cancelled";
    public const string OnlySoldProductsCanBeReturned = "product.return.only_sold";
    public const string SoldDateMustExistBeforeReturning = "product.return.sold_date_missing";
    public const string ReturnDateCannotBeEarlierThanSoldDate = "product.return.invalid_date";

    public const string SupplierIsRequired = "product.supplier.required";
    public const string CategoryIsRequired = "product.category.required";
    public const string ProductDescriptionIsRequired = "product.description.required";

    public const string AcquisitionCostIsRequired = "product.acquisition_cost.required";
    public const string AcquisitionCostInUsdIsRequired = "product.acquisition_cost_usd.required";
    public const string AcquisitionCostCurrencyMustMatchSupplier = "product.acquisition_cost.currency_mismatch";
    public const string AcquisitionCostInUsdMustUseUsd = "product.acquisition_cost_usd.must_be_usd";

    public const string AcquireDateIsRequired = "product.acquire_date.required";
}
