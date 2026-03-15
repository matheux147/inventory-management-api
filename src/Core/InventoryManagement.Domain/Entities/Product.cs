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

    public Money AcquisitionCost { get; private set; } = null!;
    public Money AcquisitionCostUsd { get; private set; } = null!;

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
        Money acquisitionCost,
        Money acquisitionCostUsd,
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
            throw new DomainException("Cancelled product cannot be sold.");

        if (Status == ProductStatus.Returned)
            throw new DomainException("Returned product cannot be sold.");

        if (Status == ProductStatus.Sold)
            throw new DomainException("Product is already sold.");

        if (soldDate < AcquireDate)
            throw new DomainException("Sold date cannot be earlier than acquire date.");

        SoldDate = soldDate;
        Status = ProductStatus.Sold;
    }

    public void Cancel(DateTime cancelDate)
    {
        if (Status == ProductStatus.Cancelled)
            throw new DomainException("Product is already cancelled.");

        if (Status == ProductStatus.Returned)
            throw new DomainException("Returned product cannot be cancelled.");

        if (cancelDate < AcquireDate)
            throw new DomainException("Cancel date cannot be earlier than acquire date.");

        if (Status == ProductStatus.Sold && SoldDate.HasValue && cancelDate < SoldDate.Value)
            throw new DomainException("Cancel date cannot be earlier than sold date.");

        CancelDate = cancelDate;
        Status = ProductStatus.Cancelled;
    }

    public void Return(DateTime returnDate)
    {
        if (Status == ProductStatus.Returned)
            throw new DomainException("Product is already returned.");

        if (Status == ProductStatus.Cancelled)
            throw new DomainException("Cancelled product cannot be returned.");

        if (Status != ProductStatus.Sold)
            throw new DomainException("Only sold products can be returned.");

        if (!SoldDate.HasValue)
            throw new DomainException("Sold date must exist before returning a product.");

        if (returnDate < SoldDate.Value)
            throw new DomainException("Return date cannot be earlier than sold date.");

        ReturnDate = returnDate;
        Status = ProductStatus.Returned;
    }

    private void SetSupplier(Supplier supplier)
    {
        Supplier = supplier ?? throw new DomainException("Supplier is required.");
        SupplierId = supplier.Id;
    }

    private void SetCategory(Category category)
    {
        Category = category ?? throw new DomainException("Category is required.");
        CategoryId = category.Id;
    }

    private void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Product description is required.");

        Description = description.Trim();
    }

    private void SetAcquisitionCosts(Money acquisitionCost, Money acquisitionCostUsd)
    {
        if (acquisitionCost is null)
            throw new DomainException("Acquisition cost is required.");

        if (acquisitionCostUsd is null)
            throw new DomainException("Acquisition cost in USD is required.");

        if (acquisitionCost.Currency != Supplier.Currency)
            throw new DomainException("Acquisition cost currency must match supplier currency.");

        if (acquisitionCostUsd.Currency != CurrencyCode.USD)
            throw new DomainException("Acquisition cost in USD must use USD currency.");

        AcquisitionCost = acquisitionCost;
        AcquisitionCostUsd = acquisitionCostUsd;
    }

    private void SetAcquireDate(DateTime acquireDate)
    {
        if (acquireDate == default)
            throw new DomainException("Acquire date is required.");

        AcquireDate = acquireDate;
    }
}
