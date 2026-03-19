using InventoryManagement.Application.Abstractions.Errors;
using InventoryManagement.Application.Products.CreateProduct;
using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Errors;
using InventoryManagement.Domain.Ports.Context;
using InventoryManagement.Domain.Ports.Gateways.Audit;
using InventoryManagement.Domain.Ports.Gateways.Wms;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.ValueObjects;
using Moq;

namespace InventoryManagement.Application.Tests.Products;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ISupplierRepository> _supplierRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IWmsGateway> _wmsGatewayMock;
    private readonly Mock<IAuditLogGateway> _auditLogGatewayMock;
    private readonly Mock<ICurrentUserContext> _userContextMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _wmsGatewayMock = new Mock<IWmsGateway>();
        _auditLogGatewayMock = new Mock<IAuditLogGateway>();
        _userContextMock = new Mock<ICurrentUserContext>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new CreateProductCommandHandler(
            _productRepositoryMock.Object,
            _supplierRepositoryMock.Object,
            _categoryRepositoryMock.Object,
            _wmsGatewayMock.Object,
            _auditLogGatewayMock.Object,
            _userContextMock.Object,
            _dateTimeProviderMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WhenSupplierDoesNotExist_ShouldReturnSupplierNotFoundError()
    {
        // Arrange
        var command = new CreateProductCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Product", 10m, 10m, DateTime.UtcNow);

        _supplierRepositoryMock
            .Setup(x => x.GetByIdAsync(command.SupplierId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Supplier?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("supplier.not_found", result.Error.Code);
        Assert.Equal(ApplicationErrors.SupplierNotFound, result.Error);
    }

    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ShouldReturnCategoryNotFoundError()
    {
        // Arrange
        var command = new CreateProductCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Product", 10m, 10m, DateTime.UtcNow);
        var supplier = Supplier.Create("Valid", Email.Create("a@b.com"), CurrencyCode.USD, CountryCode.Create("US"));

        _supplierRepositoryMock
            .Setup(x => x.GetByIdAsync(command.SupplierId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplier);

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("category.not_found", result.Error.Code);
        Assert.Equal(ApplicationErrors.CategoryNotFound, result.Error);
    }

    [Fact]
    public async Task Handle_WhenWmsGatewayFails_ShouldReturnWmsCreateFailedError()
    {
        // Arrange
        var command = new CreateProductCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Product", 10m, 10m, DateTime.UtcNow);
        var supplier = Supplier.Create("Valid", Email.Create("a@b.com"), CurrencyCode.USD, CountryCode.Create("US"));
        var category = Category.Create("Valid", CategoryShortcode.Create("CAT-1"));

        _supplierRepositoryMock
            .Setup(x => x.GetByIdAsync(command.SupplierId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplier);

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _wmsGatewayMock
            .Setup(x => x.CreateProductAsync(It.IsAny<WmsCreateProductRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<WmsCreateProductResponseDto>.Failure(IntegrationErrors.WmsCreateFailed));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("wms.create_failed", result.Error.Code);
    }

    [Fact]
    public async Task Handle_WhenAuditLogGatewayFails_ShouldReturnAuditLogCreateFailedError()
    {
        // Arrange
        var command = new CreateProductCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Product", 10m, 10m, DateTime.UtcNow);
        var supplier = Supplier.Create("Valid", Email.Create("a@b.com"), CurrencyCode.USD, CountryCode.Create("US"));
        var category = Category.Create("Valid", CategoryShortcode.Create("CAT-1"));

        _supplierRepositoryMock
            .Setup(x => x.GetByIdAsync(command.SupplierId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplier);

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _wmsGatewayMock
            .Setup(x => x.CreateProductAsync(It.IsAny<WmsCreateProductRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<WmsCreateProductResponseDto>.Success(new WmsCreateProductResponseDto("wms-123")));

        _userContextMock.Setup(x => x.UserId).Returns(Guid.NewGuid());
        _userContextMock.Setup(x => x.Email).Returns("user@test.com");
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        _auditLogGatewayMock
            .Setup(x => x.CreateEntryAsync(It.IsAny<AuditLogCreateEntryRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(IntegrationErrors.AuditLogCreateFailed));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("auditlog.create_failed", result.Error.Code);
    }

    [Fact]
    public async Task Handle_WithValidData_ShouldCreateProductAndReturnSuccess()
    {
        // Arrange
        var command = new CreateProductCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Product", 10m, 10m, DateTime.UtcNow);
        var supplier = Supplier.Create("Valid", Email.Create("a@b.com"), CurrencyCode.USD, CountryCode.Create("US"));
        var category = Category.Create("Valid", CategoryShortcode.Create("CAT-1"));

        _supplierRepositoryMock
            .Setup(x => x.GetByIdAsync(command.SupplierId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplier);

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _wmsGatewayMock
            .Setup(x => x.CreateProductAsync(It.IsAny<WmsCreateProductRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<WmsCreateProductResponseDto>.Success(new WmsCreateProductResponseDto("wms-123")));

        _userContextMock.Setup(x => x.UserId).Returns(Guid.NewGuid());
        _userContextMock.Setup(x => x.Email).Returns("user@test.com");
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        _auditLogGatewayMock
            .Setup(x => x.CreateEntryAsync(It.IsAny<AuditLogCreateEntryRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        
        _productRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
