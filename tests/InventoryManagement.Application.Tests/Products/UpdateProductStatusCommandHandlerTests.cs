using InventoryManagement.Application.Abstractions.Errors;
using InventoryManagement.Application.Products.UpdateProductStatus;
using InventoryManagement.Domain.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Errors;
using InventoryManagement.Domain.Ports.Context;
using InventoryManagement.Domain.Ports.Gateways.Audit;
using InventoryManagement.Domain.Ports.Gateways.Email;
using InventoryManagement.Domain.Ports.Gateways.Wms;
using InventoryManagement.Domain.Ports.Repositories;
using InventoryManagement.Domain.ValueObjects;
using Moq;

namespace InventoryManagement.Application.Tests.Products;

public class UpdateProductStatusCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IEmailGateway> _emailGatewayMock;
    private readonly Mock<IWmsGateway> _wmsGatewayMock;
    private readonly Mock<IAuditLogGateway> _auditLogGatewayMock;
    private readonly Mock<ICurrentUserContext> _userContextMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ChangeProductStatusCommandHandler _handler;

    public UpdateProductStatusCommandHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _emailGatewayMock = new Mock<IEmailGateway>();
        _wmsGatewayMock = new Mock<IWmsGateway>();
        _auditLogGatewayMock = new Mock<IAuditLogGateway>();
        _userContextMock = new Mock<ICurrentUserContext>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new ChangeProductStatusCommandHandler(
            _productRepositoryMock.Object,
            _emailGatewayMock.Object,
            _wmsGatewayMock.Object,
            _auditLogGatewayMock.Object,
            _userContextMock.Object,
            _dateTimeProviderMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ShouldReturnProductNotFoundError()
    {
        // Arrange
        var command = new UpdateProductStatusCommand(Guid.NewGuid(), ProductStatus.Sold, DateTime.UtcNow);

        _productRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(command.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("product.not_found", result.Error.Code);
        Assert.Equal(ApplicationErrors.ProductNotFound, result.Error);
    }

    [Fact]
    public async Task Handle_WhenValid_ShouldUpdateInternalProductAndReturnSuccess()
    {
        // Arrange
        var command = new UpdateProductStatusCommand(Guid.NewGuid(), ProductStatus.Sold, DateTime.UtcNow);
        var supplier = Supplier.Create("Valid", Email.Create("a@b.com"), CurrencyCode.USD, CountryCode.Create("US"));
        var category = Category.Create("Valid", CategoryShortcode.Create("CAT-1"));
        var product = Product.Create(supplier, category, "Test", 10m, 10m, DateTime.UtcNow.AddDays(-1));
        
        // Give reflection access or bypass for Product ID since it's generated natively
        _productRepositoryMock
            .Setup(x => x.GetByIdWithDetailsAsync(command.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _wmsGatewayMock
            .Setup(x => x.DispatchProductAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        _emailGatewayMock
            .Setup(x => x.SendAsync(It.IsAny<EmailSendRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        _auditLogGatewayMock
            .Setup(x => x.CreateEntryAsync(It.IsAny<AuditLogCreateEntryRequestDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        _userContextMock.Setup(x => x.UserId).Returns(Guid.NewGuid());
        _userContextMock.Setup(x => x.Email).Returns("admin@a.com");
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ProductStatus.Sold, product.Status);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
