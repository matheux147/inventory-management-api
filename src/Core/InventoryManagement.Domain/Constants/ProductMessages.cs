namespace InventoryManagement.Domain.Constants;

public static class ProductMessages
{
    public const string CancelledProductCannotBeSold = "Cancelled product cannot be sold.";
    public const string ReturnedProductCannotBeSold = "Returned product cannot be sold.";
    public const string ProductAlreadySold = "Product is already sold.";
    public const string SoldDateCannotBeEarlierThanAcquireDate = "Sold date cannot be earlier than acquire date.";
    
    public const string ProductAlreadyCancelled = "Product is already cancelled.";
    public const string ReturnedProductCannotBeCancelled = "Returned product cannot be cancelled.";
    public const string CancelDateCannotBeEarlierThanAcquireDate = "Cancel date cannot be earlier than acquire date.";
    public const string CancelDateCannotBeEarlierThanSoldDate = "Cancel date cannot be earlier than sold date.";
    
    public const string ProductAlreadyReturned = "Product is already returned.";
    public const string CancelledProductCannotBeReturned = "Cancelled product cannot be returned.";
    public const string OnlySoldProductsCanBeReturned = "Only sold products can be returned.";
    public const string SoldDateMustExistBeforeReturning = "Sold date must exist before returning a product.";
    public const string ReturnDateCannotBeEarlierThanSoldDate = "Return date cannot be earlier than sold date.";
    
    public const string SupplierIsRequired = "Supplier is required.";
    public const string CategoryIsRequired = "Category is required.";
    public const string ProductDescriptionIsRequired = "Product description is required.";
    
    public const string AcquisitionCostIsRequired = "Acquisition cost is required.";
    public const string AcquisitionCostInUsdIsRequired = "Acquisition cost in USD is required.";
    public const string AcquisitionCostCurrencyMustMatchSupplier = "Acquisition cost currency must match supplier currency.";
    public const string AcquisitionCostInUsdMustUseUsd = "Acquisition cost in USD must use USD currency.";
    
    public const string AcquireDateIsRequired = "Acquire date is required.";
}
