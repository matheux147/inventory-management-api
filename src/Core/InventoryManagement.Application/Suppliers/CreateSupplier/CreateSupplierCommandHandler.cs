using InventoryManagement.Application.Abstractions.Errors;
using InventoryManagement.Application.Abstractions.Messaging;
using InventoryManagement.Application.DTOs.Suppliers;
using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Exceptions;
using InventoryManagement.Domain.Ports.Context;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Application.Suppliers.CreateSupplier;

public sealed class CreateSupplierCommandHandler(
    ISupplierRepository supplierRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateSupplierCommand, SupplierResponseDto>
{
    public async Task<Result<SupplierResponseDto>> Handle(
        CreateSupplierCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var email = Email.Create(request.Email);

            var emailAlreadyExists = await supplierRepository.ExistsByEmailAsync(
                email,
                cancellationToken);

            if (emailAlreadyExists)
                return Result<SupplierResponseDto>.Failure(ApplicationErrors.SupplierEmailAlreadyExists);

            var currency = CurrencyCode.Create(request.Currency);
            var country = CountryCode.Create(request.Country);

            var supplier = Supplier.Create(
                request.Name,
                email,
                currency,
                country);

            await supplierRepository.AddAsync(supplier, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<SupplierResponseDto>.Success(supplier.Map());
        }
        catch (DomainException exception)
        {
            return Result<SupplierResponseDto>.Failure(new Error(exception.Message));
        }
    }
}