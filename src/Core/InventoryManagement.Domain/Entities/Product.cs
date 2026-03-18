using InventoryManagement.Domain.Constants;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Entities;

public class Product : Entity
{
    public Guid SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = null!;

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public decimal AcquisitionCost { get; private set; }
    public decimal AcquisitionCostUsd { get; private set; }

    public DateTime AcquireDate { get; private set; }
    public DateTime? SoldDate { get; private set; }
    public DateTime? CancelDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }

    public ProductStatus Status { get; private set; }

    private Product()
    {
    }

    public Product(
        Supplier supplier,
        Category category,
        string description,
        decimal acquisitionCost,
        decimal acquisitionCostUsd,
        DateTime acquireDate)
    {
        SetSupplier(supplier);
        SetCategory(category);
        SetDescription(description);
        SetAcquisitionCosts(acquisitionCost, acquisitionCostUsd);
        SetAcquireDate(acquireDate);

        Status = ProductStatus.Created;
    }

    public void ChangeDescription(string description)
    {
        SetDescription(description);
    }

    public void ChangeCategory(Category category)
    {
        SetCategory(category);
    }

    public void Sell(DateTime soldDate)
    {
        if (Status == ProductStatus.Cancelled)
            throw new DomainException(ProductMessages.CancelledProductCannotBeSold);

        if (Status == ProductStatus.Returned)
            throw new DomainException(ProductMessages.ReturnedProductCannotBeSold);

        if (Status == ProductStatus.Sold)
            throw new DomainException(ProductMessages.ProductAlreadySold);

        if (soldDate < AcquireDate)
            throw new DomainException(ProductMessages.SoldDateCannotBeEarlierThanAcquireDate);

        SoldDate = soldDate;
        Status = ProductStatus.Sold;
    }

    public void Cancel(DateTime cancelDate)
    {
        if (Status == ProductStatus.Cancelled)
            throw new DomainException(ProductMessages.ProductAlreadyCancelled);

        if (Status == ProductStatus.Returned)
            throw new DomainException(ProductMessages.ReturnedProductCannotBeCancelled);

        if (cancelDate < AcquireDate)
            throw new DomainException(ProductMessages.CancelDateCannotBeEarlierThanAcquireDate);

        if (Status == ProductStatus.Sold && SoldDate.HasValue && cancelDate < SoldDate.Value)
            throw new DomainException(ProductMessages.CancelDateCannotBeEarlierThanSoldDate);

        CancelDate = cancelDate;
        Status = ProductStatus.Cancelled;
    }

    public void Return(DateTime returnDate)
    {
        if (Status == ProductStatus.Returned)
            throw new DomainException(ProductMessages.ProductAlreadyReturned);

        if (Status == ProductStatus.Cancelled)
            throw new DomainException(ProductMessages.CancelledProductCannotBeReturned);

        if (Status != ProductStatus.Sold)
            throw new DomainException(ProductMessages.OnlySoldProductsCanBeReturned);

        if (!SoldDate.HasValue)
            throw new DomainException(ProductMessages.SoldDateMustExistBeforeReturning);

        if (returnDate < SoldDate.Value)
            throw new DomainException(ProductMessages.ReturnDateCannotBeEarlierThanSoldDate);

        ReturnDate = returnDate;
        Status = ProductStatus.Returned;
    }

    private void SetSupplier(Supplier supplier)
    {
        Supplier = supplier ?? throw new DomainException(ProductMessages.SupplierIsRequired);
        SupplierId = supplier.Id;
    }

    private void SetCategory(Category category)
    {
        Category = category ?? throw new DomainException(ProductMessages.CategoryIsRequired);
        CategoryId = category.Id;
    }

    private void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException(ProductMessages.ProductDescriptionIsRequired);

        Description = description.Trim();
    }

    private void SetAcquisitionCosts(decimal acquisitionCost, decimal acquisitionCostUsd)
    {
        if (acquisitionCost <= 0)
            throw new DomainException(ProductMessages.AcquisitionCostIsRequired);

        if (acquisitionCostUsd <= 0)
            throw new DomainException(ProductMessages.AcquisitionCostInUsdIsRequired);

        AcquisitionCost = acquisitionCost;
        AcquisitionCostUsd = acquisitionCostUsd;
    }

    private void SetAcquireDate(DateTime acquireDate)
    {
        if (acquireDate == default)
            throw new DomainException(ProductMessages.AcquireDateIsRequired);

        AcquireDate = acquireDate;
    }
}
