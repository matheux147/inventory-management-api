using InventoryManagement.Application.Abstractions.Errors;
using InventoryManagement.Application.Suppliers.CreateSupplier;
using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Ports.Cache;
using InventoryManagement.Domain.Ports.Context;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.ValueObjects;
using Moq;

namespace InventoryManagement.Application.Tests.Suppliers;

public class CreateSupplierCommandHandlerTests
{
    private readonly Mock<ISupplierRepository> _supplierRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IAppCache> _appCacheMock;
    private readonly CreateSupplierCommandHandler _handler;

    public CreateSupplierCommandHandlerTests()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _appCacheMock = new Mock<IAppCache>();
        _handler = new CreateSupplierCommandHandler(
            _supplierRepositoryMock.Object, 
            _unitOfWorkMock.Object,
            _appCacheMock.Object);
    }

    [Fact]
    public async Task Handle_WhenEmailExists_ShouldReturnSupplierEmailAlreadyExistsError()
    {
        // Arrange
        var command = new CreateSupplierCommand("Test Supplier", "test@test.com", "USD", "US");

        _supplierRepositoryMock
            .Setup(x => x.ExistsByEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("supplier.email_already_exists", result.Error.Code);
        Assert.Equal(ApplicationErrors.SupplierEmailAlreadyExists, result.Error);
    }

    [Fact]
    public async Task Handle_WithValidData_ShouldCreateSupplierAndReturnSuccess()
    {
        // Arrange
        var command = new CreateSupplierCommand("Test Supplier", "test@test.com", "USD", "US");

        _supplierRepositoryMock
            .Setup(x => x.ExistsByEmailAsync(It.IsAny<Email>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(command.Name, result.Value.Name);
        Assert.Equal(command.Email, result.Value.Email);
        Assert.Equal(command.Currency, result.Value.Currency);
        Assert.Equal(command.Country, result.Value.Country);
        
        _supplierRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Supplier>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
